using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PracticalAssignment.Models.BusinessEntities;
using PracticalAssignment.Models.DataEntities;

namespace PracticalAssignment.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// logs in user
        /// </summary>
        /// <returns>Login View</returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// Validates user upon login
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Login([Bind(Include = "Username,Password")]UserVM userVM)
        {
            if (userVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    string ConnectionString = "data source=INSPIRON;initial catalog=DbUnitTestingAssignment;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;";
                    using (var connection = new SqlConnection(ConnectionString))
                    {
                        string commandText = "SELECT Username FROM tblUser WHERE Username=@Username AND Password = @Password";
                        using (var command = new SqlCommand(commandText, connection))
                        {
                            command.Parameters.AddWithValue("@Username", userVM.Username);
                            command.Parameters.AddWithValue("@Password", EncryptData(userVM.Password, "AnyKey"));
                            connection.Open();

                            string userName = (string)command.ExecuteScalar();

                            if (!String.IsNullOrEmpty(userName))
                            {
                                FormsAuthentication.SetAuthCookie(userVM.Username, false);
                                return RedirectToAction("AllStudents", "Student");
                            }

                            TempData["Message"] = "Login failed.User name or password supplied doesn't exist.";
                            connection.Close();
                        }
                    }
                }

                catch (Exception)
                {
                    return View("Login");
                }
            }
            return View("Login");
        }


        //logout current user
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// this method registers user
        /// </summary>
        /// <param name="userVM"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Register(UserVM userVM)
        {
            if (userVM == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                tblUser DomainUser = new tblUser()
                {
                    Id = userVM.Id,
                    Username = userVM.Username,
                    Password = userVM.Password
                };
                using (var _context = new DbUnitTestingAssignmentEntities())
                {
                    /**
                     * this logic checks if username already exists
                     */
                    bool EmailAlreadyExists = await _context.tblUsers.AnyAsync(x => x.Username == userVM.Username);
                    if (EmailAlreadyExists)
                    {
                        ModelState.AddModelError("", "This Username Already Exists");
                        return View();
                    }
                    DomainUser.Password = EncryptData(DomainUser.Password, "AnyKey"); // Encrypts the user password
                    _context.tblUsers.Add(DomainUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Account");
                }

            }
            return View();
        }

        [NonAction]
        //Password Encryption Method
        public string EncryptData(string textData, string Encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            //set the mode for operation of the algorithm
            objrij.Mode = CipherMode.CBC;
            //set the padding mode used in the algorithm.
            objrij.Padding = PaddingMode.PKCS7;
            //set the size, in bits, for the secret key.
            objrij.KeySize = 0x80;
            //set the block size in bits for the cryptographic operation.
            objrij.BlockSize = 0x80;
            //set the symmetric key that is used for encryption & decryption.
            byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);
            //set the initialization vector (IV) for the symmetric algorithm
            byte[] EncryptionkeyBytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            //Creates symmetric AES object with the current key and initialization vector IV.
            ICryptoTransform objtransform = objrij.CreateEncryptor();
            byte[] textDataByte = Encoding.UTF8.GetBytes(textData);
            //Final transform the test string.
            return Convert.ToBase64String(objtransform.TransformFinalBlock(textDataByte, 0, textDataByte.Length));
        }


    }
}
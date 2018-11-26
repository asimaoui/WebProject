using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebScrach2.Models
{
    public class RenamePassword
    {
        [Required(ErrorMessage = "Password is required")]
        //[StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [RegularExpression(@"(?=^.{6,18}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "length must be 6 to 18 letters, contains atleast one digit, atleast one upper case and atleast one lower case and no whitespace.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [RegularExpression(@"(?=^.{6,18}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "length must be 6 to 18 letters, contains atleast one digit, atleast one upper case and atleast one lower case and no whitespace.")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Password")]
        public string ConfirmPassword { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string UserName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }

    public class LoginModel
    {
        [Required]
        //[DataType(DataType.EmailAddress)]
        [Display(Name = "Email / UserName")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "user name must be combination of letters and numbers only.")]
        public string Email { get; set; }
 
        [Required]
        //[StringLength(18, ErrorMessage = "Password .", MinimumLength = 6)]
        [RegularExpression(@"(?=^.{6,18}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "length must be 6 to 18 letters, contains atleast one digit, atleast one upper case and atleast one lower case and no whitespace.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
 
        [HiddenInput(DisplayValue=false)]
        public string ReturnUrl { get; set; }

    }

    public class MovieDetail
    {

        public Movy movie = new Movy();
        public int movieID { get; set; }
        public string comment { get; set; }

        //public List<Comment> comments =  { get; set; }

    }


    public class MovieHome
    {

        public List<Movy> movis = new List<Movy>();

        [HiddenInput(DisplayValue = false)]
        public string searchString { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int dropdownID { get; set; }


    }

    

    public class RegisterModel
    {

        [Required]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required]
        [DisplayName("Email / UserName")]
        //[DataType(DataType.Custom)]
        [RegularExpression(@"^[a-zA-Z0-9]+$" , ErrorMessage = "user name must be combination of letters and numbers only.")]  
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"(?=^.{6,18}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$", ErrorMessage = "length must be 6 to 18 letters, contains atleast one digit, atleast one upper case and atleast one lower case and no whitespace.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsAdmin { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool IsActive { get; set; }

    }

}
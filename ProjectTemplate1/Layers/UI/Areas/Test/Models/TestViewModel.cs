using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using $customNamespace$.Resources.Helpers.GeneratedResxClasses;
using $safeprojectname$.Models;

namespace $safeprojectname$.Areas.Test.Models
{
    public class TestViewModel : baseViewModel
    {
        [Display(ResourceType = typeof($customNamespace$.Resources.UserAdministration.UserAdminTexts), Name = UserAdminTextsKeys.CreationDate)]
        public DateTime SomeDate { get; set; }

        [Display(Name="Some Double Value")]
        public double SomeDouble { get; set; }

        [Display(Name="Some Float Value")]
        public float SomeFloat { get; set; }

        [Display(Name = "Some Boolean Value")]
        public bool SomeBoolean { get; set; }

        [Display(Name = "Some Boolean Nullable")]
        public bool? SomeBooleanNullable { get; set; }

        [Display(Name = "Some String Array")]
        public IEnumerable<string> SomeStringArray { get; set; }

        [Display(Name = "Another String Array")]
        public IEnumerable<string> SomeStringArrayII { get; set; }

        [Display(Name = "Another String Array")]
        public IEnumerable<string> SomeStringArrayIII { get; set; }
    }
}
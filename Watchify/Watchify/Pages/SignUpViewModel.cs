using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watchify.Shared.ViewModels;
using Watchify.Features.SignUp;

namespace Watchify.Pages
{
    public class SignUpViewModel : ViewModelBase
    {
        public SignUpFormViewModel SignUpFormViewModel { get; }

        public SignUpViewModel(SignUpFormViewModel signUpFormViewModel)
        {
            SignUpFormViewModel = signUpFormViewModel;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EntryValidatorSample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}
    async void btnValidate_Clicked(object sender, EventArgs e)
    {
      if (!ValidateForm())
      {
       await DisplayAlert("Validate", "Please enter required data", "OK");
      }
      else
      {
        await DisplayAlert("Validate", "Data saved successfully", "OK");
      }
    }
    private bool ValidateForm()
    {
      return
        VldtrName.Validate() &
        VldtrAddress.Validate() &
        VldtrEmail.Validate() &
        VldtrPhone.Validate() &
        VldtrWeight.Validate() &
        VldtrRegExp.Validate();
    }

  }
}

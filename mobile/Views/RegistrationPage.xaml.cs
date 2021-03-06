﻿using Microsoft.Win32.SafeHandles;
using mobile.Resources;
using mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        RegisterModel register = new RegisterModel();
        

        public RegistrationPage()
        {
            InitializeComponent();
            BindingContext = register;
            pckDoctor.ItemsSource = App.aum.DoctorList;
        }
        //regex for email validation
        Regex emailPattern = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        Regex passwordPattern = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$");
        bool res;
        async private void btnRegister_Clicked(object sender, EventArgs e)
        {
            Animations.Button_Scale_Clicked((Button)sender);

            // to verify if the date of birth was chosen
            int result = DateTime.Compare(dpBirthDate.Date, DateTime.Today);

            //verify if fiels are filled in
            if (string.IsNullOrWhiteSpace(entFirstName.Text) || string.IsNullOrWhiteSpace(entLastName.Text) || string.IsNullOrWhiteSpace(entEmail.Text) || string.IsNullOrWhiteSpace(entPhone.Text) || string.IsNullOrWhiteSpace(entCnp.Text)  || string.IsNullOrWhiteSpace(entPassword.Text) || string.IsNullOrWhiteSpace(entConfPwd.Text))
            {
                res = await DisplayAlert(AppResources.AlertFillField, AppResources.AlertChange, AppResources.Yes, AppResources.No);
                if( res == false)
                {
                    await this.Navigation.PushAsync(new MainPage());
                }

            }
            else if (entPassword.Text != entConfPwd.Text)
            {
                res = await DisplayAlert(AppResources.AlertPwdMatch, AppResources.AlertChange, AppResources.Yes, AppResources.No);
                if (res == false)
                {
                    await this.Navigation.PushAsync(new MainPage());
                }
            }
            else if (!emailPattern.IsMatch(entEmail.Text))
            {
                res = await DisplayAlert(AppResources.AlertEmail, AppResources.AlertChange, AppResources.Yes, AppResources.No);
                if (res == false)
                {
                    await this.Navigation.PushAsync(new MainPage());
                }
            }
            else if (!cnpCheck(entCnp.Text))
            {
                res= await DisplayAlert(AppResources.AlertPIN, AppResources.AlertChange, AppResources.Yes, AppResources.No);
                if (res == false)
                {
                    await this.Navigation.PushAsync(new MainPage());
                }
            }
            else if(Equals(dpBirthDate, DateTime.Now))
            {
                res= await DisplayAlert(AppResources.AlertBirthday, AppResources.AlertChange, AppResources.Yes, AppResources.No);
                if (res == false)
                {
                    await this.Navigation.PushAsync(new MainPage());
                }
            }
            else if (Equals(entPassword.Text, entConfPwd.Text))
            {
                if (entPassword.Text.Length < 6)
                {
                    lblPassCheck.Text = AppResources.AlertPwdShort;
                    lblPassCheck.TextColor = Color.Red;
                }
                if(passwordPattern.IsMatch(entPassword.Text))
                {
                    DoctorModel x = (DoctorModel)pckDoctor.SelectedItem;
                    register.doctorId = x.Id;
                    register.RegisterCommand.Execute(null);
                       await this.Navigation.PushAsync(new RegistrationSucceedPage());
                }
                else
                {
                    res = await DisplayAlert(AppResources.AlertPwdNotStrong, AppResources.AlertChange, AppResources.Yes, AppResources.No);
                    if (res == false)
                    {
                        Page page = new MainPage();
                        await Navigation.PushAsync(page);
                        NavigationPage.SetHasNavigationBar(page, false);
                    }
                }
                
            }
        }
        //strong password check
        private void onTexChanged(object sender, TextChangedEventArgs e)
        {
            if (entPassword.Text.Length < 6)
            {
                lblPassCheck.IsVisible = true;
                lblPassCheck.Text = AppResources.AlertPwdShort;
                lblPassCheck.TextColor = Color.Red;

            }
            else
            {
                lblPassCheck.IsVisible = true;
                if (!Regex.Match(entPassword.Text, "[a-z]", RegexOptions.ECMAScript).Success)
                {
                    lblPassCheck.Text = AppResources.PwdLowerCharacter;
                    lblPassCheck.TextColor = Color.DarkOrange;
                }
                else if (!Regex.Match(entPassword.Text, "[A-Z]", RegexOptions.ECMAScript).Success)
                {
                    lblPassCheck.Text = AppResources.PwdUpperCharacter;
                    lblPassCheck.TextColor = Color.OrangeRed;
                }
                else if (!Regex.Match(entPassword.Text, "[0-9]", RegexOptions.ECMAScript).Success)
                {

                    lblPassCheck.Text = AppResources.PwdDigit;
                    lblPassCheck.TextColor = Color.Orange;
                }
                else
                {
                    lblPassCheck.Text = AppResources.PwdStrong;
                    lblPassCheck.TextColor = Color.Green;
                }
            }
        }
        //doesn't work don't know why yet
         private bool  cnpCheck( string cnp) {

            if (cnp.Length == 13)
            {
                int gender = Convert.ToInt16(cnp.Substring(0, 1));
                int year = Convert.ToInt16(cnp.Substring(1, 2));
                int month = Convert.ToInt16(cnp.Substring(3, 2));
                int day = Convert.ToInt16(cnp.Substring(5, 2));
                int county = Convert.ToInt16(cnp.Substring(7, 2));
                int rest = Convert.ToInt16(cnp.Substring(9, 3));
                int check = Convert.ToInt32(cnp.Substring(cnp.Length - 1));
                if ((gender == 1 || gender == 2 || (gender >=5 && gender <=8)) && (county < 56 || county == 51 || county ==  52) && month <= 12 && day <= 31 &&  rest != 0  && check == checkDigit(cnp))
                {
                    if ((month == 2))
                    {
                        if(day <= 28)
                        {
                            return true;
                        }
                        if (year > 0 && year % 4 == 0 && day == 29)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                    else if ((month <= 7 && month % 2 == 0 && day <= 30) || (month <= 7 && month % 2 != 0 && day <= 31) || (month >=8 && month % 2 == 0 && day <= 31) || (month >= 7 && month % 2 != 0 && day <= 30)) // luna >8
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
         }

        private int  checkDigit( string series)
        {
            long  cod = (Convert.ToInt64(series))/10;
            List<int> checkNumber = new List<int>() {9,7,2,8,5,3,6,4,1,9,7,2};
            long sum =0;
            
            foreach(int element in checkNumber)
            {
                sum += element * (cod % 10);
                cod = cod / 10;
            }
            int suma = (int)sum % 11;
            return suma;

        }

         async private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new LogInPage());
        }
    }
    }


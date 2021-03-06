﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutUsPage : ContentPage
    {
        //comm
        public AboutUsPage()
        {
            InitializeComponent();

            BindingContext = App.aum;
            
            // added an event on item tapped 
            doctorsList.ItemTapped += OnItemTapped;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.aum.getDoctors();
       
        }
        // any item tapped will open another page with more details about the doctor
        public void OnItemTapped(object sender, ItemTappedEventArgs e) => Navigation.PushAsync(new AboutUsPageDetails((DoctorModel)e.Item));


       

    }
}
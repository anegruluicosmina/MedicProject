﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using mobile.ViewModels;

namespace mobile.ViewModels
{
    // viewModel used to do all the work needed for the HomePage
    public class HomePageModel : INotifyPropertyChanged
    {
        // used for the tapped event, to close the most recent opened item in the list
        private AppointmentModel oldAppointment;
        private bool _isLoading = true;
        public bool isLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
        public List<AppointmentModel> Aplist
        {
            get { return _aplist; }
            set
            {
                _aplist = value;
                OnPropertyChanged();
            }
        }
        private List<AppointmentModel> _aplist = new List<AppointmentModel>();

        protected virtual void OnPropertyChanged([CallerMemberName] string
            propertyName = null)

        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public HomePageModel()
        {
            getNextAppts();
        }

        public void getBackAppts()
        {
            isLoading = true;
            Task.Run(async () =>
            {
                Aplist = await App.apiServicesManager.GetBackPatientApptsAsync(App.user.token);
                isLoading = false;
                
            });
          
            
        }
        public void getNextAppts()
        {
            isLoading = true;
            Task.Run(async () =>
            {
                Aplist = await App.apiServicesManager.GetNextPatientApptsAsync(App.user.token);
                isLoading = false;

            });


        }
        public void HideOrShowAppointment(AppointmentModel a)
        {

            if (oldAppointment == a)
            {
                //click twice to hide the details
                a.IsVisible = !a.IsVisible;
                UpdateAppointments(a);

            }
            else
            {
                if (oldAppointment != null)
                {
                    // hide the old selected item details
                    oldAppointment.IsVisible = false;
                    UpdateAppointments(oldAppointment);
                }
                // show selected item details
                a.IsVisible = true;
                UpdateAppointments(a);

            }
            oldAppointment = a;

        }
        // this is used to trigger the obs collection changed items in order to show the user the changed ViewCell
        private void UpdateAppointments(AppointmentModel a)
        {
            var index = _aplist.IndexOf(a);
            if (index >= 0)
            {
                _aplist.Remove(a);
                _aplist.Insert(index, a);
                var _aplist2 = new List<AppointmentModel>();
                foreach (AppointmentModel apm in _aplist)
                    _aplist2.Add(apm);
                Aplist = _aplist2;
            }
        }

        // this method is used to enable deleting of an appointment through the button shown on the UI, from doctor
        // did like this because I cannot access directly elements of the DataTemplate
        public async void DeleteAppt()
        {

            await App.apiServicesManager.DeleteApptAsync(oldAppointment.Id);
            getNextAppts();
           
            


        }

    }

}

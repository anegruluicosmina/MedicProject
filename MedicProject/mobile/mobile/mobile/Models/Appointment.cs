﻿using System;
using System.Collections.Generic;
using System.Text;

namespace mobile
{
    // for the moment, this is a dummy class used to load data for the list view in home page
    class Appointment
    {

        private string date;
        private string hour;
        private string status;
        private const string duration = "Duration: 30 min";
        private string details;
        private string patientName;

        public string PatientName 
        { 
            get 
            {
                return patientName;
            }
            set { patientName = value; }
        }
        
        public string Details
        {
            get
            {
                return details;
            }
            set
            {
                details = value;

            }
        }
        public string Date
        {

            get
            {

                return date;

            }
            set
            {
                date = value.ToString();
            }
        }

        public string Hour
        {
            get
            {
                return hour;
            }

            set
            {

                hour = value.ToString();
            }
        }
        public string Status
        {
            get
            {
                return status;
            }

            set
            {

                status = value.ToString();
            }
        }
        public string Duration
        {

            get { return duration; }
        }
        
    }

}
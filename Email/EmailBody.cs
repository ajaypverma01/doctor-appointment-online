using DoctorPatient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorPatient.Email
{
    public class EmailBody
    {
        public string PatientName { get; set; }        
        public string DoctorName { get; set; }
        public string AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }
        public string ClinicPhoneNumber { get; set; }
        public string AppointmentID { get; set; }
        public string EmailTo { get; set; }
        public Enums.EmailType EmailType { get; set; }
    }
}

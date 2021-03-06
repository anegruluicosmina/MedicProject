using AutoMapper;
using MedicProject.DTO;
using MedicProject.Models;

namespace MedicProject.Mappers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Appointments, NextOrHistoryAppointmentsDTO>();
            CreateMap<Appointments, ReturnAppointmentsDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<User, PatientDTO>();
            CreateMap<User, DoctorDTO>();
            CreateMap<User, UserApp>();
            CreateMap<User, AccountDTO>();
            CreateMap<User, MedicDTO>();
        }
    }
}
using ClinicBookingSystem.ModelVM.Appointment;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem.Services.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ClinicDbContext clinicDbContext;

        public AppointmentService(ClinicDbContext clinicDbContext)
        {
            this.clinicDbContext = clinicDbContext;
        }
        public async Task<GetAppointmentVM> BookAppointmentAsync(CreateAppointmentVM appointmentVM, string patientId)
        {
            var doctor = await clinicDbContext.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Id == appointmentVM.DoctorId);
            if (doctor == null || !doctor.IsActive)
            {
                throw new InvalidOperationException("Doctor not found or not active.");
            }

            if (appointmentVM.AppointmentDate.Date + appointmentVM.StartTime <= DateTime.Now)
            {
                throw new InvalidOperationException("Appointment cannot be in the past.");
            }

            var endTime = appointmentVM.StartTime + TimeSpan.FromMinutes(doctor.ConsultationDurationMinutes);

            var fits = await clinicDbContext.AvailabilitySlots.AnyAsync(s =>
                s.DoctorId == doctor.Id && s.IsActive &&
                s.DayOfWeek == appointmentVM.AppointmentDate.DayOfWeek &&
                s.StartTime <= appointmentVM.StartTime && s.EndTime >= endTime);
            if (!fits)
            {
                throw new InvalidOperationException("The doctor is not available at this time.");
            }

            var doctorBusy = await clinicDbContext.Appointments.AnyAsync(a =>
                a.DoctorId == doctor.Id &&
                a.AppointmentDate == appointmentVM.AppointmentDate.Date &&
                a.Status != AppointmentStatus.Cancelled &&
                a.StartTime < endTime && appointmentVM.StartTime < a.EndTime);
            if (doctorBusy)
            {
                throw new InvalidOperationException("This time is already booked for the doctor.");
            }

                var patientBusy = await clinicDbContext.Appointments.AnyAsync(a =>
                a.PatientId == patientId &&
                a.AppointmentDate == appointmentVM.AppointmentDate.Date &&
                a.Status != AppointmentStatus.Cancelled &&
                a.StartTime < endTime && appointmentVM.StartTime < a.EndTime);
            if (patientBusy)
            {
                throw new InvalidOperationException("You already have an appointment at this time.");
            }
            //mapping to appointment entity and saving to database
            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patientId,
                AppointmentDate = appointmentVM.AppointmentDate.Date,
                StartTime = appointmentVM.StartTime,
                EndTime = endTime,
                Status = AppointmentStatus.Pending,
                Notes = appointmentVM.Notes
            };
            clinicDbContext.Appointments.Add(appointment);
            await clinicDbContext.SaveChangesAsync();

            var patient = await clinicDbContext.Users.FindAsync(patientId);
            return new GetAppointmentVM
            {
                Id = appointment.Id,
                DoctorId = doctor.Id,
                DoctorName = doctor.FullName,
                SpecialtyName = doctor.Specialty?.Name,
                PatientId = patientId,
                PatientName = patient?.FullName,
                AppointmentDate = appointment.AppointmentDate,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                Status = appointment.Status,
                Notes = appointment.Notes,
                CreatedAt = appointment.CreatedAt
            };
        }

        public async Task<bool> CancelAsync(int id)
        {
            var appointment = await clinicDbContext.Appointments.FindAsync(id);
            if (appointment == null || appointment.Status == AppointmentStatus.Cancelled || appointment.Status == AppointmentStatus.Completed)
            {
                return false;
            }

            if (appointment.AppointmentDate.Date + appointment.StartTime <= DateTime.Now)
            {
                return false;
            }

            appointment.Status = AppointmentStatus.Cancelled;
            await clinicDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteAsync(int id)
        {
            var appointment = await clinicDbContext.Appointments.FindAsync(id);
            if (appointment == null || appointment.Status != AppointmentStatus.Confirmed)
            {
                return false;
            }

            appointment.Status = AppointmentStatus.Completed;
            await clinicDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmAsync(int id)
        {
            var appointment = await clinicDbContext.Appointments.FindAsync(id);
            if (appointment == null || appointment.Status != AppointmentStatus.Pending)
            {
                return false;
            }

            appointment.Status = AppointmentStatus.Confirmed;
            await clinicDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<GetAppointmentVM>> GetAllAppointmentsAsync()
        {
            var result = await clinicDbContext.Appointments
                .Select(a => new GetAppointmentVM
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    SpecialtyName = a.Doctor.Specialty.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return result;
        }

        public async Task<GetAppointmentVM?> GetByIdAsync(int id)
        {
            return await clinicDbContext.Appointments
                .Where(a => a.Id == id)
                .Select(a => new GetAppointmentVM
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    SpecialtyName = a.Doctor.Specialty.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                })
                .FirstOrDefaultAsync();
        }
        //for role doctor
        public async Task<List<GetAppointmentVM>> GetDoctorAppointmentsAsync(int doctorId, DateTime date)
        {
            var day = date.Date;
            return await clinicDbContext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == day)
                .OrderBy(a => a.StartTime)
                .Select(a => new GetAppointmentVM
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    SpecialtyName = a.Doctor.Specialty.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();
        }
        //for patient to see his schedule
        public async Task<List<GetAppointmentVM>> GetPatientAppointmentsAsync(string patientId)
        {
            return await clinicDbContext.Appointments
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.StartTime)
                .Select(a => new GetAppointmentVM
                {
                    Id = a.Id,
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor.FullName,
                    SpecialtyName = a.Doctor.Specialty.Name,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    AppointmentDate = a.AppointmentDate,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Status = a.Status,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> RescheduleAsync(UpdateAppointmentVM appointmentVM)
        {
            var appointment = await clinicDbContext.Appointments.FindAsync(appointmentVM.Id);
            if (appointment == null || appointment.Status == AppointmentStatus.Cancelled || appointment.Status == AppointmentStatus.Completed)
            {
                return false;
            }

            var doctor = await clinicDbContext.Doctors.FirstOrDefaultAsync(d => d.Id == appointment.DoctorId);
            if (doctor == null || !doctor.IsActive)
            {
                return false;
            }

            if (appointmentVM.AppointmentDate.Date + appointmentVM.StartTime <= DateTime.Now)
            {
                return false;
            }

            var endTime = appointmentVM.StartTime + TimeSpan.FromMinutes(doctor.ConsultationDurationMinutes);

            var fits = await clinicDbContext.AvailabilitySlots.AnyAsync(s =>
                s.DoctorId == doctor.Id && s.IsActive &&
                s.DayOfWeek == appointmentVM.AppointmentDate.DayOfWeek &&
                s.StartTime <= appointmentVM.StartTime && s.EndTime >= endTime);
            if (!fits)
            {
                return false;
            }

            var day = appointmentVM.AppointmentDate.Date;
            //ملهمش لازمه
            var doctorBusy = await clinicDbContext.Appointments.AnyAsync(a =>
                a.Id != appointment.Id &&
                a.DoctorId == doctor.Id &&
                a.AppointmentDate == day &&
                a.Status != AppointmentStatus.Cancelled &&
                a.StartTime < endTime && appointmentVM.StartTime < a.EndTime);
            if (doctorBusy)
            {
                return false;
            }
            //بردو ملوش لازمه

            var patientBusy = await clinicDbContext.Appointments.AnyAsync(a =>
                a.Id != appointment.Id &&
                a.PatientId == appointment.PatientId &&
                a.AppointmentDate == day &&
                a.Status != AppointmentStatus.Cancelled &&
                a.StartTime < endTime && appointmentVM.StartTime < a.EndTime);
            if (patientBusy)
            {
                return false;
            }

            appointment.AppointmentDate = day;
            appointment.StartTime = appointmentVM.StartTime;
            appointment.EndTime = endTime;
            appointment.Notes = appointmentVM.Notes;

            await clinicDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<TimeSpan>> GetAvailableTimesAsync(int doctorId, DateTime date)
        {
            var doctor = await clinicDbContext.Doctors.FirstOrDefaultAsync(d => d.Id == doctorId);
            if (doctor == null || !doctor.IsActive || doctor.ConsultationDurationMinutes <= 0)
                return new List<TimeSpan>();

            var day = date.Date;
            var step = TimeSpan.FromMinutes(doctor.ConsultationDurationMinutes);

            var slots = await clinicDbContext.AvailabilitySlots
                .Where(s => s.DoctorId == doctorId && s.IsActive && s.DayOfWeek == day.DayOfWeek)
                .OrderBy(s => s.StartTime)
                .ToListAsync();

            var booked = await clinicDbContext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentDate == day
                    && a.Status != AppointmentStatus.Cancelled)
                .Select(a => new { a.StartTime, a.EndTime })
                .ToListAsync();

            var result = new List<TimeSpan>();
            foreach (var slot in slots)
            {
                for (var time = slot.StartTime; time + step <= slot.EndTime; time += step)
                {
                    var end = time + step;
                    if (day == DateTime.Today && day + time <= DateTime.Now)
                        continue;
                    var overlaps = booked.Any(b => time < b.EndTime && b.StartTime < end);
                    if (!overlaps && !result.Contains(time))
                        result.Add(time);
                }
            }
            return result.OrderBy(t => t).ToList();
        }





        public async Task<DoctorBookingInfoVM?> GetDoctorBookingInfoAsync(int doctorId)
        {
            var doctor = await clinicDbContext.Doctors
                .Include(d => d.Specialty)
                .FirstOrDefaultAsync(d => d.Id == doctorId);
            if (doctor == null || !doctor.IsActive)
                return null;

            var schedule = await clinicDbContext.AvailabilitySlots
                .Where(s => s.DoctorId == doctorId && s.IsActive)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .Select(s => new DoctorScheduleVM
                {
                    Day = s.DayOfWeek.ToString(),
                    StartTime = s.StartTime.ToString(@"hh\:mm"),
                    EndTime = s.EndTime.ToString(@"hh\:mm")
                })
                .ToListAsync();

            return new DoctorBookingInfoVM
            {
                DoctorId = doctor.Id,
                DoctorName = doctor.FullName,
                SpecialtyName = doctor.Specialty != null ? doctor.Specialty.Name : null,
                Bio = doctor.Bio,
                ConsultationDurationMinutes = doctor.ConsultationDurationMinutes,
                Schedule = schedule
            };
        }


    }
}

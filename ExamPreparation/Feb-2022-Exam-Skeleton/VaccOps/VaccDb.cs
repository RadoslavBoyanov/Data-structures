namespace VaccOps
{
    using Models;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VaccDb : IVaccOps
    {
        private Dictionary<string, Doctor> doctorsByNames = new Dictionary<string, Doctor>();
        private Dictionary<string, Patient> patientsByNames = new Dictionary<string, Patient>();

        public void AddDoctor(Doctor doctor)
        {
            if (doctorsByNames.ContainsKey(doctor.Name))
            {
                throw new ArgumentException();
            }

            doctorsByNames.Add(doctor.Name, doctor);
        }

        public void AddPatient(Doctor doctor, Patient patient)
        {
            if (!doctorsByNames.ContainsKey(doctor.Name))
            {
                throw new ArgumentException();
            }

            patientsByNames.Add(patient.Name, patient);
            patientsByNames[patient.Name].Doctor = doctor;
            doctorsByNames[doctor.Name].Patients.Add(patient);
        }

        public void ChangeDoctor(Doctor oldDoctor, Doctor newDoctor, Patient patient)
        {
            if (!doctorsByNames.ContainsKey(oldDoctor.Name) || !doctorsByNames.ContainsKey(newDoctor.Name) || !patientsByNames.ContainsKey(patient.Name))
            {
                throw new ArgumentException();
            }

            doctorsByNames[oldDoctor.Name].Patients.Remove(patient);
            doctorsByNames[newDoctor.Name].Patients.Add(patient);
            patientsByNames[oldDoctor.Name].Doctor = newDoctor; 
        }

        public bool Exist(Doctor doctor)
        {
            return doctorsByNames.ContainsKey(doctor.Name);
        }

        public bool Exist(Patient patient)
        {
            return patientsByNames.ContainsKey(patient.Name);
        }

        public IEnumerable<Doctor> GetDoctors()
        {
            return doctorsByNames.Values;
        }

        public IEnumerable<Doctor> GetDoctorsByPopularity(int populariry)
            => this.doctorsByNames.Values.Where(doc => doc.Popularity == populariry);

        public IEnumerable<Doctor> GetDoctorsSortedByPatientsCountDescAndNameAsc()
            => this.doctorsByNames.Values.OrderByDescending(doc => doc.Patients.Count).ThenBy(doc => doc.Name);

        public IEnumerable<Patient> GetPatients()
        {
            return patientsByNames.Values;
        }

        public IEnumerable<Patient> GetPatientsByTown(string town)
            => this.patientsByNames.Values.Where(pat => pat.Town == town);

        public IEnumerable<Patient> GetPatientsInAgeRange(int lo, int hi)
            => this.patientsByNames.Values.Where(pat => pat.Age >= lo && pat.Age <= hi);

        public IEnumerable<Patient> GetPatientsSortedByDoctorsPopularityAscThenByHeightDescThenByAge()
            => this.patientsByNames.Values.OrderBy(pat => pat.Doctor.Popularity)
                                          .ThenByDescending(pat => pat.Height)
                                          .ThenBy(pat => pat.Age);

        public Doctor RemoveDoctor(string name)
        {
            if (!doctorsByNames.ContainsKey(name))
            {
                throw new ArgumentException();
            }

            var doctor = doctorsByNames[name];

            foreach (var patient in doctor.Patients)
            {
                patientsByNames.Remove(patient.Name);
            }

            doctorsByNames.Remove(name);

            return doctor;
        }
    }
}

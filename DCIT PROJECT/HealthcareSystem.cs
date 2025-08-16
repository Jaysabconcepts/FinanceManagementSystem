using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareSystem
{
    // a. Generic Repository
    public class Repository<T>
    {
        private List<T> items = new();

        public void Add(T item) => items.Add(item);

        public List<T> GetAll() => new List<T>(items);

        public T? GetById(Func<T, bool> predicate) =>
            items.FirstOrDefault(predicate);

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // b. Patient class
    public class Patient
    {
        public int Id;
        public string Name;
        public int Age;
        public string Gender;

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString() =>
            $"Patient ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
    }

    // c. Prescription class
    public class Prescription
    {
        public int Id;
        public int PatientId;
        public string MedicationName;
        public DateTime DateIssued;

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString() =>
            $"Prescription ID: {Id}, Patient ID: {PatientId}, Medication: {MedicationName}, Date: {DateIssued.ToShortDateString()}";
    }

    // g. HealthSystemApp
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo = new();
        private Repository<Prescription> _prescriptionRepo = new();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new();

        public void SeedData()
        {
            // Add Patients
            _patientRepo.Add(new Patient(1, "John Doe", 30, "Male"));
            _patientRepo.Add(new Patient(2, "Jane Smith", 28, "Female"));
            _patientRepo.Add(new Patient(3, "Michael Johnson", 45, "Male"));

            // Add Prescriptions
            _prescriptionRepo.Add(new Prescription(1, 1, "Amoxicillin", DateTime.Now.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Paracetamol", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Ibuprofen", DateTime.Now.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Vitamin C", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(5, 3, "Aspirin", DateTime.Now.AddDays(-2)));
        }

        public void BuildPrescriptionMap()
        {
            _prescriptionMap.Clear();
            foreach (var prescription in _prescriptionRepo.GetAll())
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();

                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("\n--- Patient List ---");
            foreach (var patient in _patientRepo.GetAll())
            {
                Console.WriteLine(patient);
            }
        }

        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            return _prescriptionMap.ContainsKey(patientId)
                ? _prescriptionMap[patientId]
                : new List<Prescription>();
        }

        public void PrintPrescriptionsForPatient(int id)
        {
            Console.WriteLine($"\n--- Prescriptions for Patient ID: {id} ---");
            var prescriptions = GetPrescriptionsByPatientId(id);
            if (prescriptions.Count == 0)
            {
                Console.WriteLine("No prescriptions found.");
                return;
            }
            foreach (var p in prescriptions)
            {
                Console.WriteLine(p);
            }
        }

        public static void Run()
        {
            var app = new HealthSystemApp();
            app.SeedData();
            app.BuildPrescriptionMap();
            app.PrintAllPatients();

            // Pick one patient to show prescriptions
            Console.Write("\nEnter Patient ID to view prescriptions: ");
            if (int.TryParse(Console.ReadLine(), out int pid))
            {
                app.PrintPrescriptionsForPatient(pid);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }
    }
}

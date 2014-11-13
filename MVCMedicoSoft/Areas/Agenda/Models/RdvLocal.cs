using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCMedicoSoft.Areas.Agenda.Models
{
    public class RdvLocal
    {
        
        private readonly RendezVous _instance;

        public RdvLocal(RendezVous rdv)
        {
            this._instance = rdv;
        }


        //Heures-date
        public DateTime FinRdv
        {
            get { return _instance.FinRdv; }
            set { _instance.FinRdv = value; }
        }

        public DateTime DebutRdv
        {
            get { return _instance.DebutRdv; }
            set { _instance.DebutRdv = value; }
        }
        //nom du medecin
        public string NomMedecin
        {
            get { return _instance.Medecin.Nom+" "+_instance.Medecin.Prenom; }
        }


        //nom du patient
        public string NomPatient
        {
            get { return _instance.Patient.Nom + " " + _instance.Patient.Prenom; }
        }
        //Description
        public string Description
        {
            get { return _instance.Description; }
            set { _instance.Description = value; }
        }
        //Nom du secretaire
        public string NomSecretaire
        {
            get { return _instance.Secretaire.Nom + " " + _instance.Secretaire.Prenom; }
        }
    }
}
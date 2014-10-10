using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// Classe représentant un rendez-vous
    /// </summary>
    public class RendezVous
    {
        #region Fields
        private int _idRdv;
        private DateTime _debutRdv;
        private DateTime _finRdv;
        private string _description;
        private Medecin _medecin;
        private Personne _patient;
        private Secretaire _secretaire;        
        #endregion

        #region Properties
        public Personne Patient
        {
          get { return _patient; }
          set { _patient = value; }
        }
        public Secretaire Secretaire
        {
            get { return _secretaire; }
            set { _secretaire = value; }
        }

        public Medecin Medecin
        {
            get { return _medecin; }
            set { _medecin = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public DateTime FinRdv
        {
            get { return _finRdv; }
            set { _finRdv = value; }
        }

        public DateTime DebutRdv
        {
            get { return _debutRdv; }
            set { _debutRdv = value; }
        }

        public int IdRdv
        {
            get { return _idRdv; }
            set { _idRdv = value; }
        } 
        #endregion

        public static RendezVous getInfo(int idRdv)
        {
             List<Dictionary<string, object>> infoRdv = GestionConnexion.Instance.getData("Select * from RendezVous where idRdv=" + idRdv);
            RendezVous retour = new RendezVous();
            foreach (Dictionary<string, object> item in infoRdv)
            {
                retour.Medecin = Medecin.getInfo(item["FKInami"].ToString());
                retour.Secretaire= Secretaire.getInfo(int.Parse(item["FKIdSecretaire"].ToString()));
                retour.Patient = Personne.getInfo(item["FKNumRegNational"].ToString());
                retour.IdRdv = idRdv;
                DateTime temp = DateTime.Parse(item["dateRdv"].ToString());
                retour.DebutRdv = temp;
               
            }
            return retour;
        }
    }
}

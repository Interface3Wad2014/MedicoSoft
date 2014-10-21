using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    public enum TypeOfUser { Patient, Medecin, Secretaire }
    /// <summary>
    /// Classe représentant une Personne dans le business case : MedicoDB
    /// </summary>
    public class Personne
    {
        #region Fields
        
            private string _numRegNational;
            private string _nom;
            private string _prenom;
            private DateTime _dateNaissance;
            private char _sexe;
            private string _adresse;
            private int? _codePostal;//Nullable<int> ==> permet de définir un type comme étant nullable
            private string _ville;
            private string _adresseMail;
            private string _pays = "Belgium";
            private string _telFixe;
            private string _telMobile;
         
        #endregion 
        #region Properties
            /// <summary>
            /// Permet de stocker le numéro de registre national
            /// </summary>
            public string NumRegNational  
            {
                get{ return _numRegNational;} 
                set{ 
                        if(value.Length==12)
                            _numRegNational=value;
                    }
            }
        /// <summary>
            /// Permet de stocker le nom
        /// </summary>
            public string Nom
            {
                get { return _nom; }
                set { _nom = value; }
            }

        /// <summary>
            /// Permet de stocker le prénom
        /// </summary>
            public string Prenom
            {
                get { return _prenom; }
                set { _prenom = value; }
            }

        /// <summary>
            /// Permet de stocker la date de naissance
        /// </summary>
            public DateTime DateNaissance
            {
                get { return _dateNaissance; }
                set { _dateNaissance = value; }
            }

        /// <summary>
            /// Permet de stocker le sexe (M/F/N)
        /// </summary>
            public char Sexe
            {
                get { return _sexe; }
                set { _sexe = value; }
            }
        /// <summary>
            /// Permet de stocker l'adresse
        /// </summary>
            public string Adresse
            {
                get { return _adresse; }
                set { _adresse = value; }
            }
        /// <summary>
            /// Permet de stocker le code postal
        /// </summary>
            public int? CodePostal
            {
                get { return _codePostal; }
                set { _codePostal = value; }
            }
        /// <summary>
            /// Permet de stocker la ville
        /// </summary>
            public string Ville
            {
                get { return _ville; }
                set { _ville = value; }
            }
        /// <summary>
            /// Permet de stocker l'adresse e-mail
        /// </summary>
            public string AdresseMail
            {
                get { return _adresseMail; }
                set { _adresseMail = value; }
            }
        /// <summary>
            /// Permet de stocker le pays
        /// </summary>
            public string Pays
            {
                get { return _pays; }
                set { _pays = value; }
            }
        /// <summary>
            /// Permet de stocker le téléphone fixe
        /// </summary>
            public string TelFixe
            {
                get { return _telFixe; }
                set { _telFixe = value; }
            }

        /// <summary>
            /// Permet de stocker le téléphone mobile
        /// </summary>
            public string TelMobile
            {
                get { return _telMobile; }
                set { _telMobile = value; }
            }
        #endregion
        #region Constructors
            /// <summary>
            /// Constructeur ridicule pour le besoin de l'héritage
            /// ou d'une construction par défaut
            /// </summary>
            public Personne() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numRegNational">le numéro de registre national</param>
        /// <param name="nom">le nom</param>
        /// <param name="prenom">le prenom</param>
        /// <param name="dateNaissance"> la date de naissance</param>
        /// <param name="sexe"> M ou F ou N</param>
            public Personne(string numRegNational, string nom, string prenom, DateTime dateNaissance, char sexe)
            {
                this.NumRegNational = numRegNational;
                this.Nom = nom;
                this.Prenom = prenom;
                this.DateNaissance = dateNaissance;
                this.Sexe = sexe;
            }
        
        /// <summary>
           /// Constructeur par défaut de ma personne
           /// </summary>
           /// <param name="numRegNational">le numéro de registre national</param>
           /// <param name="nom">le nom</param>
           /// <param name="prenom">le prenom</param>
           /// <param name="dateNaissance"> la date de naissance</param>
           /// <param name="sexe"> M ou F ou N</param>
           /// <param name="adresse"> rue + numero</param>
           /// <param name="codePostal"> uniquement BElge</param>
           /// <param name="ville">La ville</param>
           /// <param name="adresseMail">l'email</param>
           /// <param name="pays">Belgium par défaut si null</param>
           /// <param name="telFixe">format (+32)xxx-xxxxxx</param>
            /// <param name="telMobile">format (+32)xxxx-xxxxxx</param>
            public Personne(string numRegNational, string nom, string prenom, DateTime dateNaissance, char sexe, string adresse = default(string), int? codePostal = null, string ville = default(string), string adresseMail = default(string), string pays = "Belgium", string telFixe = default(string), string telMobile = default(string))
                : this(numRegNational, nom, prenom, dateNaissance, sexe)
            {
               /* this.NumRegNational = numRegNational;
                this.Nom = nom;
                this.Prenom = prenom;
                this.DateNaissance = dateNaissance;
                this.Sexe = sexe;*/
                this.Adresse = adresse;
                this.CodePostal = codePostal;
                this.Ville = ville;
                this.AdresseMail = adresseMail;
                this.Pays = pays;
                this.TelFixe = telFixe;
                this.TelMobile = telMobile;
            }
        #endregion
        #region Methode Static
        /// <summary>
        /// Permet de récupérer une personne de la DB via son identifiant
        /// </summary>
        /// <param name="identifiant">l'identifiant de la personne (registre national)</param>
        /// <param name="p">La personne a remplir</param>
        /// <returns>Une instance de personne completée par les infos de la DB</returns>
          public static Personne getInfo(string identifiant, Personne p=null)
          {
              //1 - Créér mon objet connection
             // SqlConnection oConn = new SqlConnection(@"Server=MIKE-PC\TFTIC;Database=MedicoDB;User Id=MedicoUser;Password=medicopass;");
                //ou
              SqlConnection oConn = new SqlConnection(DAL.Properties.Settings.Default.ConnectionString);
              //2 - Connectez-vous
              try
              {
                  oConn.Open();

                  //3- Construction de ma requête
                  string query = @"select * from Personne 
                                where numRegNational='" + identifiant + "'";
                  //4- Création de notre SqlDataReader
                  SqlDataReader oDr ;

                  //5- Creation la command
                  SqlCommand oCmd = new SqlCommand(query, oConn);

                  //6- Exécution de la command
                  oDr = oCmd.ExecuteReader();

                  //7- Parcourir les données renvoyées
                  //7.1 - Y'a-t-il des données ??
                  if (oDr.HasRows)//retourne true si min 1 ligne dans notre reader
                  {
                    //7.2- Boucle de parcours
                      while(oDr.Read()) //tant que je sais lire des données
                      {
                          //7.2.1- Construction de l'objet personne
                           p =p?? new Personne();
                          p.NumRegNational = oDr["numRegNational"].ToString();
                          // ou p.NumRegNational = oDr.GetString(0);  
                          // ou p.NumRegNational= oDr[0].ToString();
                            p.Nom = oDr["nom"].ToString();
                            p.Prenom = oDr["prenom"].ToString();
                            p.DateNaissance =DateTime.Parse(oDr["dateNaissance"].ToString());
                            p.Sexe = oDr["sexe"].ToString()[0];
                            p.Adresse = oDr["adresse"].ToString();
                            if (oDr["codePostal"].ToString() != "") p.CodePostal = int.Parse(oDr["codePostal"].ToString());
                           
                            p.Ville = oDr["ville"].ToString();
                            p.AdresseMail = oDr["adresseMail"].ToString(); 
                            p.Pays = oDr["pays"].ToString(); 
                            p.TelFixe = oDr["telFixe"].ToString(); 
                            p.TelMobile = oDr["telMobile"].ToString();

                          //7.3 - Fermer le datareader
                            oDr.Close();
                          //8- renvoi de la personne
                            return p;
                      }
                  }
                  else
                  {
                      //7.3 Pas de données donc fermeture 
                      // du datareader et renvoit de null
                      oDr.Close();
                      return null;
                  }
              }
              catch (Exception)
              {
                  return null;
              }

              return null;
          }
       
        #endregion
        #region Functions
          /// <summary>
          /// Peremet de sauvegarder une personne dans la DB
          /// </summary>
          /// <returns>true si l'insertion est OK</returns> 
          public virtual bool saveMe()
          {
              //Requête
              Personne p = Personne.getInfo(this.NumRegNational);
              string query = "";
              if(p == null)
              { 

              query = @"INSERT INTO [MedicoDB].[dbo].[Personne]
     VALUES
           (@numRegNational,@nom,@prenom,@dateNaissance,@sexe,@adresse, 
           @codePostal,@adresseMail,@pays,@telFixe,@telMobile,@ville)";
              
              }
              else
              {
                  query = @"UPDATE [MedicoDB].[dbo].[Personne]
                                        SET [nom] = @nom,
                                            [prenom] = @prenom,
                                            [dateNaissance] = @dateNaissance,
                                            [sexe] = @sexe,
                                            [adresse] = @adresse,
                                            [codePostal] = @codePostal,
                                            [adresseMail] = @adresseMail,
                                            [pays] = @pays,
                                            [telFixe] = @telFixe,
                                            [telMobile] = @telMobile,
                                            [ville] = @ville
                                            WHERE [numRegNational] = @numRegNational";
              }

              //les données a insérer
              Dictionary<string, object> valeurs = new Dictionary<string, object>();
              valeurs.Add("numRegNational", this.NumRegNational);
              valeurs.Add("nom", this.Nom);
              valeurs.Add("prenom", this.Prenom);
              valeurs.Add("dateNaissance", this.DateNaissance);
              valeurs.Add("sexe", this.Sexe);
              //Si l'adresse = default(string) , j'insère la valeur DBNULL 
              // dans les paramètres pour mettre le champs à NULL dans la DB
              valeurs.Add("adresse", this.Adresse == default(string) ? DBNull.Value : (object)this.Adresse);
              //Si le code postal à une valeur, je l'insère sinon, j'insère DBNULL
              valeurs.Add("codePostal", this.CodePostal.HasValue ? (object)this.CodePostal : DBNull.Value);
              valeurs.Add("ville", this.Ville == default(string) ? DBNull.Value : (object)this.Ville);
              valeurs.Add("adresseMail", this.AdresseMail == default(string) ? DBNull.Value : (object)this.AdresseMail);
              valeurs.Add("pays", this.Pays);
              valeurs.Add("telFixe", this.TelFixe == default(string) ? DBNull.Value : (object)this.TelFixe);
              valeurs.Add("telMobile", this.TelMobile == default(string) ? DBNull.Value : (object)this.TelMobile);



              if (GestionConnexion.Instance.saveData(query, GenerateKey.APP, valeurs))
              {
                  return true;
              }
              else
              {
                  return false;
              }
          }

          public bool getReferent(out string NomMedecin)
          {
            List<Dictionary<string, object>> ret =  GestionConnexion.Instance.getData(@"SELECT  'Dr.' +  Personne.nom as Nom
FROM         Medecin INNER JOIN
                      Utilisateur ON Medecin.FKIdUtilisateur = Utilisateur.idUtilisateur INNER JOIN
                      Personne ON Utilisateur.FkRegistreNational = Personne.numRegNational
where Medecin.INAMI=(SELECT     Medecin.INAMI
FROM         Medecin INNER JOIN
                      Patient ON Medecin.INAMI = Patient.FkINAMI INNER JOIN
                      Personne ON Patient.FknumRegNational = Personne.numRegNational
                      where numRegNational=" + this.NumRegNational + ")");

            if (ret.Count > 0) NomMedecin = ret[0][Nom].ToString();
            else NomMedecin = default(string);
              return ret.Count > 0;


          }
        #endregion





          
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// Classe décrivant un utilisateur
    /// <seealso cref="DAL.Personne"/>
    /// </summary>
    public class Utilisateur:Personne
    {
        #region Fields
        private int _idUtilisateur;
        private string _login;
        private string _password;
        private DateTime _dateCreation;
        private bool _isActif;

       
        #endregion
        #region Properties
        /// <summary>
        /// Identifiant de l'utilisateur
        /// </summary>
        public int IdUtilisateur
        {
            get { return _idUtilisateur; }
            set { _idUtilisateur = value; }
        }
        /// <summary>
        /// Login de l'utilisateur
        /// </summary>
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }
        /// <summary>
        /// Mot de passe de l'utilisateur
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public bool IsActif
        {
            get { return _isActif; }
            set { _isActif = value; }
        }
        /// <summary>
        /// Date de création de l'utilisateur
        /// </summary>
        public DateTime DateCreation
        {
            get { return _dateCreation; }
            set { _dateCreation = value; }
        } 
       
        
        
        #endregion
        #region Constructors
        /// <summary>
        /// Constructeur pour la compatibilité d'héritage
        /// </summary>
        public Utilisateur() 
        { }
        /// <summary>
        /// Constructeur Public permettant de construire un 
        /// utilisateur MAIS sans lui donner d'idUtilisateur 
        /// puisque celui-ci est calculé côté DB
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="dateCreation"></param>
        /// <param name="numRegNational"></param>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="dateNaissance"></param>
        /// <param name="sexe"></param>
        /// <param name="adresse"></param>
        /// <param name="codePostal"></param>
        /// <param name="ville"></param>
        /// <param name="adresseMail"></param>
        /// <param name="pays"></param>
        /// <param name="telFixe"></param>
        /// <param name="telMobile"></param>
        public Utilisateur(
           string login, string password,
           DateTime dateCreation,
           string numRegNational,
           string nom, string prenom,
           DateTime dateNaissance, char sexe,
           string adresse = default(string),
           int? codePostal = null,
           string ville = null,
           string adresseMail = null,
           string pays = "Belgium",
           string telFixe = null,
           string telMobile = null)
            : base(numRegNational,
                nom, prenom, dateNaissance, sexe, adresse,
                codePostal, ville, adresseMail, pays, telFixe,
                telMobile)
        {
            
            this.Login = login;
            this.Password = password;
            this.DateCreation = dateCreation;
        }
        
        /// <summary>
        /// Constructeur interne permettant de spécifier l'idUtilisateur récupéré de la base de donnée
        /// </summary>
        /// <param name="idUtilisateur"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="dateCreation"></param>
        /// <param name="numRegNational"></param>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="dateNaissance"></param>
        /// <param name="sexe"></param>
        /// <param name="adresse"></param>
        /// <param name="codePostal"></param>
        /// <param name="ville"></param>
        /// <param name="adresseMail"></param>
        /// <param name="pays"></param>
        /// <param name="telFixe"></param>
        /// <param name="telMobile"></param>
        private Utilisateur(int idUtilisateur, 
            string login, string password,
            DateTime dateCreation, 
            string numRegNational, 
            string nom,string prenom, 
            DateTime dateNaissance, char sexe, 
            string adresse=default(string), 
            int? codePostal=null,
            string ville = default(string),
            string adresseMail = default(string), 
            string pays="Belgium",
            string telFixe = default(string),
            string telMobile = default(string))
            : base(numRegNational, 
            nom,prenom,dateNaissance, sexe, adresse, 
             codePostal, ville,adresseMail, pays,telFixe, 
            telMobile)
        {
            this.IdUtilisateur = idUtilisateur;
            this.Login = login;
            this.Password = password;
            this.DateCreation = dateCreation;
        }

        #endregion

        #region Function
            /// <summary>
            /// Permet de récupérer une instance utilisateur garnie à partir de la DB
            /// </summary>
            /// <param name="idUser">identifiant de l'utilisateur</param>
            /// <param name="u">L'utilisateur a remplir</param>
            /// <returns>un utilisateur complet si l'utilisateur </returns>
            public static Utilisateur getInfo(int idUser, Utilisateur u=null)
            {
               List<Dictionary<string, object>> infoUser = GestionConnexion.Instance.getData("Select * from Utilisateur where idUtilisateur=" + idUser);
               u=u?? new Utilisateur();
               foreach (Dictionary<string, object> item in infoUser)
               {
                   Personne.getInfo(item["FkRegistreNational"].ToString(), u as Personne);
                   u.IdUtilisateur = idUser;
                   u.Login = item["loginUtilisateur"].ToString();
                   u.Password = item["passwordUtilisateur"].ToString();
                   u.DateCreation = (DateTime)item["dateCreation"];                  
               }
               return u;

            }
            /// <summary>
            /// Permet de récupérer une instance utilisateur garnie à partir de la DB
            /// </summary>
            /// <param name="login">login de l'utilisateur</param>
            /// <param name="u">L'utilisateur a remplir</param>
            /// <returns>un utilisateur complet si l'utilisateur </returns>
            public static Utilisateur getInfo(string login, Utilisateur u = null)
            {
                List<Dictionary<string, object>> infoUser = GestionConnexion.Instance.getData("Select * from Utilisateur where loginUtilisateur=" + login);
                u = u ?? new Utilisateur();
                foreach (Dictionary<string, object> item in infoUser)
                {
                    Personne.getInfo(item["FkRegistreNational"].ToString(), u as Personne);
                    u.IdUtilisateur =(int)item["idUtilisateur"];
                    u.Login = item["loginUtilisateur"].ToString();
                    u.Password = item["passwordUtilisateur"].ToString();
                    u.DateCreation = (DateTime)item["dateCreation"];
                }
                return u;

            }



            /// <summary>
            /// Permet de récupérer toutes les utilisateurs de la DB classée 
            /// </summary> 
            /// <returns>tous les utilisateurs de la DB</returns>
            public static List<Utilisateur> getInfos()
            {
                List<Dictionary<string, object>> infoUser = GestionConnexion.Instance.getData("Select * from Utilisateur");
                List<Utilisateur> retour = new List<Utilisateur>();
                foreach (Dictionary<string, object> item in infoUser)
                {
                    Utilisateur u = new Utilisateur();
                    u.Login = item["loginUtilisateur"].ToString();
                    u.Password = item["passwordUtilisateur"].ToString();
                    u.DateCreation = DateTime.Parse(item["dateCreation"].ToString());
                    u.IdUtilisateur =(int)item["idUtilisateur"];
                    Personne.getInfo(item["FkRegistreNational"].ToString(), u as Personne);
                    retour.Add(u);
                }
                return retour;
            } 
           
            /// <summary>
            /// Permet de sauver l'utilisateur courant dans la DB. Après la sauvegarde
            /// l'idutilisateur est rempli par le numéro généré par la DB
            /// <seealso cref="Personne.saveMe"/><seealso cref="GestionConnexion.getLastGenerateId"/>
            /// </summary>
            /// <returns>True si l'enregistrement s'est déroulé correctement</returns>
            public override bool saveMe()
            {
                if (base.saveMe())
                {
                    string query="";
                    if (this.IdUtilisateur == 0)
                    {
                        //Requête
                        query = @"INSERT INTO [MedicoDB].[dbo].[Utilisateur]
           ([loginUtilisateur]
           ,[passwordUtilisateur]
           ,[dateCreation]
           ,[FkRegistreNational])
     VALUES           (@loginUtilisateur, @passwordUtilisateur, @dateCreation,@FkRegistreNational)";
                    }
                    else
                    {
                        //Requête
                        query = @"UPDATE [MedicoDB].[dbo].[Utilisateur]
                               SET [loginUtilisateur] = @loginUtilisateur 
                                  ,[passwordUtilisateur] = @passwordUtilisateur 
                                  ,[dateCreation] = @dateCreation 
                                  ,[FkRegistreNational] = @FkRegistreNational 
                             WHERE [idUtilisateur] =@idUtilisateur";
                    }
                    //les données a insérer
                    Dictionary<string, object> valeurs = new Dictionary<string, object>();
                    valeurs.Add("loginUtilisateur", this.Login);
                    valeurs.Add("passwordUtilisateur", this.Password);
                    valeurs.Add("dateCreation", this.DateCreation);
                    valeurs.Add("FkRegistreNational", this.NumRegNational);


                    if (GestionConnexion.Instance.saveData(query, GenerateKey.DB, valeurs))
                    {
                        //Appel de la fonction permettant de récupérer le dernier id généré
                        // par la base de données
                      this.IdUtilisateur= GestionConnexion.Instance.getLastGenerateId();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else
                {
                    return false;
                }

            }
            /// <summary>
            /// Permet de récupérer un utilisateur via son login et son password
            /// <code>
            /// Console.Clear();
            ///    Console.WriteLine(Design.getMessageConnect());
            ///    Console.Write("Login");
            ///    string login = Console.ReadLine();
            ///    Console.Write("Password");
            ///    string password = Console.ReadLine();
            ///
            ///    Utilisateur u = Utilisateur.AuthentifieMoi(login, password);
            ///    if(u!=null)
            ///    {
            ///        FicheUtilisateur(u);
            ///    }
            ///    else
            ///    {
            ///        Console.WriteLine("Désolé. Votre combinaison login/password est invalide" );
            ///    }
            /// </code>
            /// </summary>
            /// <param name="login">le login de l'utilisateur</param>
            /// <param name="password">le password de l'utilisateur</param>
            /// <returns>Si Login/password sont correct, une instance utilisateur garnies avec les infos de la DB. sinon NULL</returns>
            public static Utilisateur AuthentifieMoi(string login, string password)
            {
                List<Dictionary<string, object>> infoUser = GestionConnexion.Instance.getData("Select * from Utilisateur where loginUtilisateur='" + login + "' and passwordUtilisateur='" + password + "'");
                Utilisateur retour = null;
                if (infoUser.Count > 0)
                {
                    int iduser = (int)infoUser[0]["idUtilisateur"];
                    retour = Utilisateur.getInfo(iduser);
                }
                return retour;
            }

            /// <summary>
            /// Permet de savoir si l'utilisateur est medecin ou secretaire ou Patient
            /// </summary>
            /// <returns>retourne le type d'utilisateur</returns>
            public TypeOfUser getRole()
            {
                //vérification si c'est un medecin
                if (Medecin.Exists(this)) return TypeOfUser.Medecin;
                if (Secretaire.Exists(this)) return TypeOfUser.Secretaire;
                return TypeOfUser.Patient;

            }
        #endregion

        

        
    }
}

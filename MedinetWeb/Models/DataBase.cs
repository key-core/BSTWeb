using System.Data.Entity.Validation;
namespace MedinetWeb.Models
{
    public class DataBase
    {
        private readonly MedinetWebEntities _databaseContext = new MedinetWebEntities();

        public void Save()
        {
            try
            {
                _databaseContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    //foreach (var ve in eve.ValidationErrors)
                    //{
                    //    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                    //        ve.PropertyName, ve.ErrorMessage);
                    //}
                }
            }
        }

        public IRepository<MW_Aprobaciones> Aprobaciones
        {
            get { return new Repository<MW_Aprobaciones>(_databaseContext); }
        }

        public IRepository<MW_AprobacionesUsuarios> AprobacionesUsuarios
        {
            get { return new Repository<MW_AprobacionesUsuarios>(_databaseContext); }
        }

        public IRepository<MW_ActivEspeciales> ActivEspeciales
        {
            get { return new Repository<MW_ActivEspeciales>(_databaseContext); }
        }

        public IRepository<MW_BeneficioVisitas> BeneficioVisitas
        {
            get { return new Repository<MW_BeneficioVisitas>(_databaseContext); }
        }

        public IRepository<MW_CadenaFarmacias> CadenaFarmacias
        {
            get { return new Repository<MW_CadenaFarmacias>(_databaseContext); }
        }

        public IRepository<MW_CaracteristicaVisitas> CaracteristicaVisitas
        {
            get { return new Repository<MW_CaracteristicaVisitas>(_databaseContext); }
        }

        public IRepository<MW_Cargos> Cargos
        {
            get { return new Repository<MW_Cargos>(_databaseContext); }
        }

        public IRepository<MW_Ciclos> Ciclos
        {
            get { return new Repository<MW_Ciclos>(_databaseContext); }
        }

        public IRepository<MW_ClasificacionFarmacias> ClasificacionFarmacias
        {
            get { return new Repository<MW_ClasificacionFarmacias>(_databaseContext); }
        }

        public IRepository<MW_ClasificacionMedicos> ClasificacionMedicos
        {
            get { return new Repository<MW_ClasificacionMedicos>(_databaseContext); }
        }

        public IRepository<MW_ConceptoDescuentos> ConceptoDescuentos
        {
            get { return new Repository<MW_ConceptoDescuentos>(_databaseContext); }
        }

        public IRepository<MW_ConsideracionMedicos> ConsideracionMedicos
        {
            get { return new Repository<MW_ConsideracionMedicos>(_databaseContext); }
        }

        public IRepository<MW_ConvenioFarmacias> ConvenioFarmacias
        {
            get { return new Repository<MW_ConvenioFarmacias>(_databaseContext); }
        }

        public IRepository<MW_DeporteMedicos> DeporteMedicos
        {
            get { return new Repository<MW_DeporteMedicos>(_databaseContext); }
        }

        public IRepository<MW_DescuentosUsuarios> DescuentosUsuarios
        {
            get { return new Repository<MW_DescuentosUsuarios>(_databaseContext); }
        }

        public IRepository<MW_Droguerias> Droguerias
        {
            get { return new Repository<MW_Droguerias>(_databaseContext); }
        }

        public IRepository<MW_DrogueriasAlmacenes> DrogueriasAlmacenes
        {
            get { return new Repository<MW_DrogueriasAlmacenes>(_databaseContext); }
        }

        public IRepository<MW_DrogueriasProductos> DrogueriasProductos
        {
            get { return new Repository<MW_DrogueriasProductos>(_databaseContext); }
        }

        public IRepository<MW_Especialidades> Especialidades
        {
            get { return new Repository<MW_Especialidades>(_databaseContext); }
        }

        public IRepository<MW_EsquemasPromocionales> EsquemasPromocionales
        {
            get { return new Repository<MW_EsquemasPromocionales>(_databaseContext); }
        }

        public IRepository<MW_RegionesEstados> RegionesEstados
        {
            get { return new Repository<MW_RegionesEstados>(_databaseContext); }
        }

        public IRepository<MW_Farmacias> Farmacias
        {
            get { return new Repository<MW_Farmacias>(_databaseContext); }
        }

        public IRepository<MW_FichasAcceso> FichasAcceso
        {
            get { return new Repository<MW_FichasAcceso>(_databaseContext); }
        }

        public IRepository<MW_Galenicas> Galenicas
        {
            get { return new Repository<MW_Galenicas>(_databaseContext); }
        }

        public IRepository<MW_Hospitales> Hospitales
        {
            get { return new Repository<MW_Hospitales>(_databaseContext); }
        }

        public IRepository<MW_IdiomaMedicos> IdiomaMedicos
        {
            get { return new Repository<MW_IdiomaMedicos>(_databaseContext); }
        }

        public IRepository<MW_Instituciones> Instituciones
        {
            get { return new Repository<MW_Instituciones>(_databaseContext); }
        }

        public IRepository<MW_Laboratorios> Laboratorios
        {
            get { return new Repository<MW_Laboratorios>(_databaseContext); }
        }

        public IRepository<MW_LaboratoriosDroguerias> LaboratoriosDroguerias
        {
            get { return new Repository<MW_LaboratoriosDroguerias>(_databaseContext); }
        }

        public IRepository<MW_Lineas> Lineas
        {
            get { return new Repository<MW_Lineas>(_databaseContext); }
        }

        public IRepository<MW_LineasEspecialidades> LineasEspecialidades
        {
            get { return new Repository<MW_LineasEspecialidades>(_databaseContext); }
        }

        public IRepository<MW_LocacionPaises> LocacionPaises
        {
            get { return new Repository<MW_LocacionPaises>(_databaseContext); }
        }

        public IRepository<MW_LocacionEstados> LocacionEstados
        {
            get { return new Repository<MW_LocacionEstados>(_databaseContext); }
        }

        public IRepository<MW_LocacionBricks> LocacionBricks
        {
            get { return new Repository<MW_LocacionBricks>(_databaseContext); }
        }

        public IRepository<MW_Marcas> Marcas
        {
            get { return new Repository<MW_Marcas>(_databaseContext); }
        }

        public IRepository<MWS_MarcasSIDEX> MarcasSIDEX
        {
            get { return new Repository<MWS_MarcasSIDEX>(_databaseContext); }
        }

        public IRepository<MW_MarcasCompetencias> MarcasCompetencias
        {
            get { return new Repository<MW_MarcasCompetencias>(_databaseContext); }
        }

        public IRepository<MW_MarcasLineas> MarcasLineas
        {
            get { return new Repository<MW_MarcasLineas>(_databaseContext); }
        }

        public IRepository<MW_Medicos> Medicos
        {
            get { return new Repository<MW_Medicos>(_databaseContext); }
        }

        public IRepository<MW_MotivoVisitas> MotivoVisitas
        {
            get { return new Repository<MW_MotivoVisitas>(_databaseContext); }
        }

        public IRepository<MW_OcupacionMedicos> OcupacionMedicos
        {
            get { return new Repository<MW_OcupacionMedicos>(_databaseContext); }
        }

        public IRepository<MW_Patologias> Patologias
        {
            get { return new Repository<MW_Patologias>(_databaseContext); }
        }

        public IRepository<MW_Posiciones> Posiciones
        {
            get { return new Repository<MW_Posiciones>(_databaseContext); }
        }

        public IRepository<MW_Productos> Productos
        {
            get { return new Repository<MW_Productos>(_databaseContext); }
        }

        public IRepository<MW_Publicaciones> Publicaciones
        {
            get { return new Repository<MW_Publicaciones>(_databaseContext); }
        }

        public IRepository<MW_Regiones> Regiones
        {
            get { return new Repository<MW_Regiones>(_databaseContext); }
        }

        public IRepository<MW_Roles> Roles
        {
            get { return new Repository<MW_Roles>(_databaseContext); }
        }

        public IRepository<MW_ServicioHospitales> ServicioHospitales
        {
            get { return new Repository<MW_ServicioHospitales>(_databaseContext); }
        }

        public IRepository<MW_SolicitudVisitas> SolicitudVisitas
        {
            get { return new Repository<MW_SolicitudVisitas>(_databaseContext); }
        }

        public IRepository<MW_TipoMedicos> TipoMedicos
        {
            get { return new Repository<MW_TipoMedicos>(_databaseContext); }
        }

        public IRepository<MW_TipoVisitaFarmacias> TipoVisitaFarmacias
        {
            get { return new Repository<MW_TipoVisitaFarmacias>(_databaseContext); }
        }

        public IRepository<MW_TipoVisitaHospitales> TipoVisitaHospitales
        {
            get { return new Repository<MW_TipoVisitaHospitales>(_databaseContext); }
        }

        public IRepository<MW_Umbrales> Umbrales
        {
            get { return new Repository<MW_Umbrales>(_databaseContext); }
        }

        public IRepository<MW_Usuarios> Usuarios
        {
            get { return new Repository<MW_Usuarios>(_databaseContext); }
        }

        public IRepository<MW_UsuariosResponsabilidades> UsuariosResponsabilidades
        {
            get { return new Repository<MW_UsuariosResponsabilidades>(_databaseContext); }
        }

        public IRepository<MW_UsuariosRoles> UsuariosRoles
        {
            get { return new Repository<MW_UsuariosRoles>(_databaseContext); }
        }

        public IRepository<MW_VisitadoresHistorial> VisitadoresHistorial
        {
            get { return new Repository<MW_VisitadoresHistorial>(_databaseContext); }
        }

        public IRepository<MW_Zonas> Zonas
        {
            get { return new Repository<MW_Zonas>(_databaseContext); }
        }

        public IRepository<MWS_AprDistribucion> AprDistribucion
        {
            get { return new Repository<MWS_AprDistribucion>(_databaseContext); }
        }

        public IRepository<MWS_ArticulosRechazados> ArticulosRechazados
        {
            get { return new Repository<MWS_ArticulosRechazados>(_databaseContext); }
        }
        
        public IRepository<MWS_CierreDistribucion> CierreDistribucion
        {
            get { return new Repository<MWS_CierreDistribucion>(_databaseContext); }
        }

        public IRepository<MWS_ConfigDistribucion> ConfigDistribucion
        {
            get { return new Repository<MWS_ConfigDistribucion>(_databaseContext); }
        }

        public IRepository<MWS_Configuraciones> ConfiguracionesSIDEX
        {
            get { return new Repository<MWS_Configuraciones>(_databaseContext); }
        }

        public IRepository<MWS_ExistenciasRechazadas> ExistenciasRechazadas
        {
            get { return new Repository<MWS_ExistenciasRechazadas>(_databaseContext); }
        }

        public IRepository<MWS_ValidaVisitadores> ValidaVisitadores
        {
            get { return new Repository<MWS_ValidaVisitadores>(_databaseContext); }
        }
    }
}
using CommunityToolkit.Mvvm.Input;
using Proyecto.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Proyecto.ViewModels
{
    public enum Ventanas { Ver, Agregar, Editar, Eliminar,Detalle }
    public class AlbumviewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Ventanas Vista { get; set; }=Ventanas.Ver;
        public string Error { get; set; } = "";
        public AlbumModels? Albumbb { get; set; }
        public ObservableCollection<AlbumModels> Album { get; set; } = new ObservableCollection<AlbumModels>();
        public ICommand CambiarVistaCommand { get; set; }
        public ICommand VerAgregarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand VerEditarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand VerEliminarCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public AlbumviewModel()
        {
            AgregarCommand= new RelayCommand(Agregar);
            CancelarCommand= new RelayCommand(Cancelar);
            VerAgregarCommand = new RelayCommand(VerAgregar);
            CambiarVistaCommand = new RelayCommand<Ventanas>(CambiarVista);
            VerEditarCommand = new RelayCommand(VerEditar);
            EditarCommand = new RelayCommand(Editar);
            VerEliminarCommand = new RelayCommand(VerEliminar);
            EliminarCommand=new RelayCommand(Eliminar);
            Deserializar();
        }
        void Agregar ()
        {
            if (Albumbb != null)
            {
                if (string.IsNullOrEmpty(Albumbb.Nombre))
                {
                    Error = "Escriba el nombre del album";
                }
                if (string.IsNullOrEmpty(Albumbb.NombreArtista))
                {
                    Error = "Escriba el nombre del artista o grupo";
                }
                if (string.IsNullOrEmpty(Albumbb.CancionPopular))
                {
                    Error = "Escriba el nombre de la cancion";
                }
                if (Albumbb.TiotalCanciones == 0)
                {
                    Error = "Escriba la cantidad de canciones del album";
                }
                if (Albumbb.AñoLanzamiento == 0)
                {
                    Error = "Escriba el año de lanzamiento yyyy (2024)";
                }
                if (string.IsNullOrEmpty(Albumbb.URLVideo))
                {
                    Error = "Escriba la URL del video en youtube";
                }
                if (!string.IsNullOrEmpty(Error))
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
                    return;
                }
                Album.Add(Albumbb);
                CambiarVista(Ventanas.Ver);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                Serializar();
            }
        }
        void Cancelar ()
        {
            CambiarVista(Ventanas.Ver);
            Error = "";
            Albumbb=null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        void VerAgregar ()
        {
            Albumbb = new AlbumModels ();
            Error = "";
            CambiarVista(Ventanas.Agregar);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        int posicion = 0;
        void VerEditar ()
        {
            if (Albumbb != null)
            {
                AlbumModels clon = new AlbumModels()
                {
                    Nombre=Albumbb.Nombre,
                    NombreArtista=Albumbb.NombreArtista,
                    TiotalCanciones=Albumbb.TiotalCanciones,
                    AñoLanzamiento=Albumbb.AñoLanzamiento,
                    URLImagen=Albumbb.URLImagen,
                    CancionPopular=Albumbb.CancionPopular,
                    URLVideo=Albumbb.URLVideo,
                };
                posicion = Album.IndexOf(Albumbb);
                Albumbb = clon;
                CambiarVista(Ventanas.Editar);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Albumbb)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
            }
        }
        void Editar()
        {
            if (Albumbb != null)
            {
                if (string.IsNullOrEmpty(Albumbb.Nombre))
                {
                    Error = "Escriba el nombre del album";
                }
                if (string.IsNullOrEmpty(Albumbb.NombreArtista))
                {
                    Error = "Escriba el nombre del artista o grupo";
                }
                if (string.IsNullOrEmpty(Albumbb.CancionPopular))
                {
                    Error = "Escriba el nombre de la cancion";
                }
                if (Albumbb.TiotalCanciones == 0)
                {
                    Error = "Escriba la cantidad de canciones del álbum";
                }
                if (Albumbb.AñoLanzamiento == 0)
                {
                    Error = "Escriba el año de lanzamiento yyyy (2024)";
                }
                if (string.IsNullOrEmpty(Albumbb.URLVideo))
                {
                    Error = "Escriba la URL del video en youtube";
                }
                if (!string.IsNullOrEmpty(Error))
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Error)));
                    return;
                }
                Album[posicion]=Albumbb;
                Serializar();
                CambiarVista(Ventanas.Ver);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
                
            }

        }
        void CambiarVista(Ventanas vista)
        {
            Vista = vista;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        void VerEliminar()
        {
            if (Albumbb != null)
            {
                CambiarVista(Ventanas.Eliminar);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista)));
            }
        }
        void Eliminar()
        {
            if (Albumbb != null)
            {
                Album.Remove(Albumbb);
                CambiarVista(Ventanas.Ver);
                Serializar();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Albumbb)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Vista))); 
            }
        }
        void Serializar()
        {
            File.WriteAllText("Album.json", JsonSerializer.Serialize(Album));
        }
        void Deserializar()
        {
            try
            {
                var datos = JsonSerializer.Deserialize<ObservableCollection<AlbumModels>>(File.ReadAllText("Album.json"));
                if (datos != null)
                {
                    foreach (var albumbb in datos)
                    {
                        Album.Add(albumbb);
                    }
                }
            }
            catch { }
        }
    }
}

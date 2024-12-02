using System;
using System.Collections.Generic;
using System.Linq;
using GestionUniversidad.Models;

public class UsuarioController
{
    private List<Usuario> _usuarios;

    public UsuarioController(List<Usuario> usuarios)
    {
        _usuarios = usuarios ?? new List<Usuario>();
    }

    public Usuario BuscarPorQR(string qr)
    {
        return _usuarios.FirstOrDefault(u => u.QR == qr);
    }

    public List<Usuario> ListarTodos()
    {
        return _usuarios;
    }
}

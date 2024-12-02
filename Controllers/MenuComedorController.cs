using System;
using System.Collections.Generic;
using System.Linq;
using GestionUniversidad.Models;
using QRCoder; //librería para la parte de QR
using System.IO;

public class MenuController
{
    private List<MenuComedor> _menus;

    public MenuController(List<MenuComedor> menus)
    {
        _menus = menus ?? new List<MenuComedor>();
    }

    public List<MenuComedor> BuscarPorDia(string dia)
    {
        if (string.IsNullOrWhiteSpace(dia))
            return _menus;

        return _menus.Where(m => NormalizarTexto(m.Dia).Contains(NormalizarTexto(dia))).ToList();
    }

    public List<MenuComedor> FiltrarPorAlergenos(string alergenos)
    {
        if (string.IsNullOrWhiteSpace(alergenos))
            return _menus;

        return _menus.Where(m => NormalizarTexto(m.Alergenos).Contains(NormalizarTexto(alergenos))).ToList();
    }

    public List<MenuComedor> ListarTodos()
    {
        return _menus;
    }

    private string NormalizarTexto(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        texto = texto.Replace("\u00A0", " ").Replace("\u200B", "");
        texto = string.Join(" ", texto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        return texto.Trim().ToLowerInvariant();
    }

    public string GenerarQRParaPago(MenuComedor menu, string alumno)
    {
        if (menu == null || string.IsNullOrWhiteSpace(alumno)) //Comprobación de que el menú y el alumno no estén vacíos
            throw new ArgumentException("El menú y el nombre del alumno no pueden ser nulos o vacíos.");

        // Contenido del QR
        string contenidoQR = $"Día: {menu.Dia}, Menú: {menu.TipoMenu}, Precio: 4.5€, Alumno: {alumno}";

        // Generar un identificador único basado en el contenido (simulando el QR)
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(contenidoQR));
    }

}

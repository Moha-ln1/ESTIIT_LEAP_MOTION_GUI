using System;
using System.Collections.Generic;
using System.Linq;
using GestionUniversidad.Models;

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
}

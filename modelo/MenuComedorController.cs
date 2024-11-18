using System;
using System.Collections.Generic;
using System.Linq;

public class MenuComedorController
{
    private List<MenuComedor> _menus;

    public MenuComedorController(List<MenuComedor> menus)
    {
        _menus = menus ?? new List<MenuComedor>();
    }

    public List<MenuComedor> BuscarPorDia(string dia)
    {
        return _menus.Where(m => m.Dia.Equals(dia, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<MenuComedor> FiltrarPorAlergenos(string alergenos)
    {
        return _menus.Where(m => m.Alergenos.Contains(alergenos, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public MenuComedor ObtenerMenuPorTipo(string tipoMenu)
    {
        return _menus.FirstOrDefault(m => m.TipoMenu.Equals(tipoMenu, StringComparison.OrdinalIgnoreCase));
    }
}

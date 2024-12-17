using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using gerenciadorTarefas.Models;
using gerenciadorTarefas.Data;
using Microsoft.EntityFrameworkCore;

namespace gerenciadorTarefas.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext appDbContext;

    public HomeController(AppDbContext appDbContext)
    {
        this.appDbContext = appDbContext;
    }

    public IActionResult Index(string id)
    {
        var filtros = new Filtros(id);

        ViewBag.Filtros = filtros;
        ViewBag.Categorias = appDbContext.Categorias.ToList();
        ViewBag.Status = appDbContext.Statuses.ToList();
        ViewBag.VencimentoValores = Filtros.VencimentoValoresFiltro;

        IQueryable<Tarefa> consulta = appDbContext.Tarefas
            .Include(c => c.Categoria)
            .Include(s => s.Status);

        if (filtros.TemCategoria)
        {
            consulta = consulta.Where(t => t.CategoriaId == filtros.CategoriaId);
        }

        if (filtros.TemStatus)
        {
            consulta = consulta.Where(t => t.StatusId == filtros.StatusId);
        }

        if (filtros.TemVencimento)
        {
            var hoje = DateTime.Today;

            if (filtros.EPassado)
            {
                consulta = consulta.Where(t => t.DataDeVencimento < hoje);
            }

            if (filtros.EFuturo)
            {
                consulta = consulta.Where(t => t.DataDeVencimento > hoje);
            }

            if (filtros.EHoje)
            {
                consulta = consulta.Where(t => t.DataDeVencimento == hoje);
            }
        }

        var tarefas = consulta.OrderBy(t => t.DataDeVencimento).ToList();
        return View(tarefas);
    }

    public IActionResult Adicionar()
    {
        ViewBag.Categorias = appDbContext.Categorias.ToList();
        ViewBag.Status = appDbContext.Statuses.ToList();

        var tarefa = new Tarefa { StatusId = "aberto" };

        return View(tarefa);
    }

    [HttpPost]
    public IActionResult Filtrar(string[] filtro)
    {
        string id = string.Join('-', filtro);
        return RedirectToAction("Index", new { ID = id });
    }

    [HttpPost]
    public IActionResult MarcarCompleto([FromRoute] string id, Tarefa tarefaSelecionada)
    {
        tarefaSelecionada = appDbContext.Tarefas.Find(tarefaSelecionada.Id);

        if (tarefaSelecionada != null)
        {
            tarefaSelecionada.StatusId = "completo";
            appDbContext.SaveChanges();
        }

        return RedirectToAction("Index", new { ID = id });
    }

    [HttpPost]
    public IActionResult Adicionar(Tarefa tarefa)
    {
        if (ModelState.IsValid)
        {
            appDbContext.Tarefas.Add(tarefa);
            appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.Categorias = appDbContext.Categorias.ToList();
            ViewBag.Status = appDbContext.Statuses.ToList();

            return View(tarefa);
        }
    }

    [HttpPost]
    public IActionResult DeletarCompletos(string id)
    {
        var paraDeletar = appDbContext.Tarefas.Where(s => s.StatusId == "completo").ToList();

        foreach (var tarefa in paraDeletar)
        {
            appDbContext.Tarefas.Remove(tarefa);
        }

        appDbContext.SaveChanges();

        return RedirectToAction("Index", new { ID = id });
    }

    public IActionResult NovaCategoria(){
        return View("AdicionarCategoria");
    }

    [HttpPost]
    public IActionResult CreateCategoria(Categoria categoria){
        try
        {
            categoria.CategoriaId = categoria.Nome.ToLower();
            appDbContext.Categorias.Add(categoria);
            appDbContext.SaveChanges();
            return RedirectToAction("ListarCategoria");
        }
        catch (System.Exception)
        {
            return View("Index");
        }
    }

    [HttpGet]
    public IActionResult ListarCategoria()
    {
        var categorias = appDbContext.Categorias.ToList();
        return View("GerenciarCategoria" , categorias);
    }

    [HttpGet]
    public IActionResult EditCategoriaView(string id){
        var categoria = appDbContext.Categorias.FirstOrDefault(c => c.CategoriaId == id);

        return View("EditarCategoria", categoria);
    }

    [HttpPost]
    public IActionResult EditCategoria(Categoria categoria)
    {
        try
        {
            appDbContext.Update(categoria);
            appDbContext.SaveChanges();
            return RedirectToAction("ListarCategoria");
        }
        catch (Exception ex)
        {
            return View("ListarCategoria");
        }
    }

    [HttpPost]
    public IActionResult DeletarCategoria(string id)
    {
        try
        {
            var categoria = appDbContext.Categorias.First(c => c.CategoriaId == id);
            appDbContext.Categorias.Remove(categoria);
            appDbContext.SaveChanges();

            return RedirectToAction("ListarCategoria");
        }
        catch (Exception ex)
        {
            return RedirectToAction("ListarCategoria");
        }
    }

    [HttpGet]
    public IActionResult EditTarefaView(int id)
    {
        try
        {
            var tarefa = appDbContext.Tarefas.Include(t => t.Categoria).Include(t => t.Status).First(t => t.Id == id);

            ViewBag.Categorias = appDbContext.Categorias.ToList();
            ViewBag.Status = appDbContext.Statuses.ToList();

            return View("EditarTarefa", tarefa);
        }
        catch
        {
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult EditTarefa(Tarefa tarefa)
    {
        try
        {
            appDbContext.Update(tarefa);
            appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        catch
        {
            ViewBag.Categorias = appDbContext.Categorias.ToList();
            ViewBag.Status = appDbContext.Statuses.ToList();
            return View("EditarTarefa", tarefa);
        }
    }

}

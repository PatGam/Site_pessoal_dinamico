﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Site_v3_dinamico.Data;
using Site_v3_dinamico.Models;

namespace Site_v3_dinamico.Controllers
{
    [Authorize(Roles ="Administradora, Cliente")]
    public class EncomendasController : Controller
    {
        private readonly SiteDinamicoBdContext _context;

        public EncomendasController(SiteDinamicoBdContext context)
        {
            _context = context;
        }

        // GET: Encomendas
        //[Authorize (Roles="Administradora")]
        public async Task<IActionResult> Index(string nomePesquisar, int pagina = 1)
        {
            //Contador de novas encomendas
            int conta = ContarNovas();
            string novas = "";
            if (conta == 0)
            {
                novas = "Não tem";
            }
            if (conta > 0 )
            {
                novas = conta.ToString();
            }
            ViewBag.Message = novas;

            //Paginacao Admin
            Paginacao paginacao1 = new Paginacao
            {
                TotalItems = await _context.Encomenda.Where(p => nomePesquisar == null || p.Cliente.Nome.Contains(nomePesquisar)).CountAsync(),
                PaginaAtual = pagina
            };

            List<Encomenda> encomendas1 = await _context.Encomenda.Where(p => nomePesquisar == null || p.Cliente.Nome.Contains(nomePesquisar))
                .Include(p => p.Servicos)
                .Include(p => p.Cliente)
                .OrderBy(p => p.respondido)
                .ThenByDescending(p => p.dataEncomenda)
                .Skip(paginacao1.ItemsPorPagina * (pagina - 1))
                .Take(paginacao1.ItemsPorPagina)
                .ToListAsync();

            ListaEncomendasViewModel modelo = new ListaEncomendasViewModel
            {
       
                Encomenda = encomendas1,
                Paginacao = paginacao1,
                NomePesquisar = nomePesquisar

            };

            //Paginação cliente
            Paginacao paginacao2 = new Paginacao
            {
                TotalItems = await _context.Encomenda.Include(p => p.Cliente).Where(p => p.Cliente.Email == User.Identity.Name).CountAsync(),
                PaginaAtual = pagina
            };

            List<Encomenda> encomendas2 = await _context.Encomenda
                .Include(p => p.Servicos)
                .Include(p => p.Cliente).Where(p => p.Cliente.Email == User.Identity.Name)
                .OrderByDescending(p => p.dataEncomenda)
                .Skip(paginacao2.ItemsPorPagina * (pagina - 1))
                .Take(paginacao2.ItemsPorPagina)
                .ToListAsync();           

            ListaEncomendasViewModel modelo2 = new ListaEncomendasViewModel
            {
                
                Encomenda = encomendas2,
                Paginacao = paginacao2,
            };

            var siteDinamicoBdContext = _context.Encomenda.Include(e => e.Cliente).Include(e => e.Servicos);

            if (User.IsInRole("Administradora"))
            {
                return base.View(modelo);
            }

            else
            {
                return base.View(modelo2);
            }
        }

        public int ContarNovas()
        {
            int count = 0;
            foreach (var item in _context.Encomenda.ToList())
            {
                //SystemsCount 
                count = _context.Encomenda.Where(x => x.respondido == false).Count();
            }
            return count;
        }
        

        // GET: Encomendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.Encomenda
                .Include(p => p.Servicos)
                .Include(p => p.Cliente)
                .SingleOrDefaultAsync(p => p.EncomendaId == id); 
            if (encomenda == null)
            {
                return NotFound();
            }

            return View(encomenda);
        }

        [Authorize(Roles = "Cliente")]
        // GET: Encomendas/Create
        public IActionResult Create()
        {
            //ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Email");
            ViewData["ServicosId"] = new SelectList(_context.Servicos, "ServicosId", "Nome");
            return View();
        }

        // POST: Encomendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Create([Bind("EncomendaId,dataEncomenda,ServicosId, detalhes")] Encomenda encomenda)
        {

            if (ModelState.IsValid)
            {
                var cliente = _context.Cliente.SingleOrDefault(c => c.Email == User.Identity.Name);
                encomenda.Cliente = cliente;
                encomenda.dataEncomenda = DateTime.Now;
                _context.Add(encomenda);
                await _context.SaveChangesAsync();
                ViewBag.Mensagem = "Pedido de orçamento realizado com sucesso.";
                return View("Sucesso");
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Email", encomenda.ClienteId);
            ViewData["ServicosId"] = new SelectList(_context.Servicos, "ServicosId", "Nome", encomenda.ServicosId);
            return View(encomenda);
        }

        [Authorize(Roles = "Administradora")]
        // GET: Encomendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.Encomenda
               .Include(e => e.Cliente)
               .Include(e => e.Servicos)
               .FirstOrDefaultAsync(m => m.EncomendaId == id);
            if (encomenda == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Email", encomenda.ClienteId);
            ViewData["ServicosId"] = new SelectList(_context.Servicos, "ServicosId", "Nome", encomenda.ServicosId);
            return View(encomenda);
        }

        // POST: Encomendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EncomendaId,dataEncomenda,ClienteId,ServicosId, respondido, detalhes")] Encomenda encomenda)
        {
            if (id != encomenda.EncomendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encomenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncomendaExists(encomenda.EncomendaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
                ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Email", encomenda.ClienteId);
                ViewData["ServicosId"] = new SelectList(_context.Servicos, "ServicosId", "Nome", encomenda.ServicosId);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Email", encomenda.ClienteId);
            ViewData["ServicosId"] = new SelectList(_context.Servicos, "ServicosId", "Nome", encomenda.ServicosId);
            return View(encomenda);
        }

        [Authorize(Roles = "Administradora")]
        // GET: Encomendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.Encomenda
                .Include(e => e.Cliente)
                .Include(e => e.Servicos)
                .FirstOrDefaultAsync(m => m.EncomendaId == id);
            if (encomenda == null)
            {
                return NotFound();
            }

            return View(encomenda);
        }

        // POST: Encomendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encomenda = await _context.Encomenda.FindAsync(id);
            _context.Encomenda.Remove(encomenda);
            await _context.SaveChangesAsync();
            ViewBag.Mensagem = "Encomenda apagada com sucesso";
            return View("Sucesso");
        }

        private bool EncomendaExists(int id)
        {
            return _context.Encomenda.Any(e => e.EncomendaId == id);
        }

        public int NumeroEncomendas()
        {
            int count = 0;
            foreach (var item in _context.Encomenda.ToList())
            {
                //SystemsCount 
                count = _context.Encomenda.Where(x => x.respondido == false).Count();
            }
            return count;
        }
    }
}

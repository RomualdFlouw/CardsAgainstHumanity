using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CardsAgainstHumanity.Models;
using CardsAgainstHumanity.Models.Repositories;
using AutoMapper;
using CardsAgainstHumanity.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace CardsAgainstHumanity.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class DecksController : ControllerBase
    {
        private readonly CardsAgainstHumanityAPIContext _context;
        private readonly IDeckRepo deckRepo;
        private readonly IMapper mapper;

        public DecksController(IDeckRepo _deckRepo, IMapper _mapper, CardsAgainstHumanityAPIContext context)
        {
            _context = context;
            this.deckRepo = _deckRepo;
            this.mapper = _mapper;
        }

        // GET: api/Decks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deck>>> GetDeck()
        {
            try
            {
                var decks = await deckRepo.GetAllAsync();

                var decksDTO = mapper.Map<IEnumerable<DeckDTO>>(decks);

                return Ok(decksDTO);
            }
            catch
            {
                return BadRequest("Something went wrong getting the decks: ");
            }
        }

        // GET: api/Decks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Deck>> GetDeck(Guid id)
        {
            try
            {
                var deck = await deckRepo.GetDecksWithCards(id);

                var deckDTO = mapper.Map<DeckDTO>(deck);

                return Ok(deckDTO);
            }
            catch
            {
                return BadRequest("Something went wrong getting the deck.");
            }
        }

        // PUT: api/Decks/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeck(Guid id, Deck deck)
        {
            if (id != deck.DeckID)
            {
                return BadRequest();
            }

            _context.Entry(deck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeckExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Decks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Deck>> PostDeck(Deck deck)
        {
            _context.Deck.Add(deck);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeck", new { id = deck.DeckID }, deck);
        }

        // DELETE: api/Decks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Deck>> DeleteDeck(Guid id)
        {
            var deck = await _context.Deck.FindAsync(id);
            if (deck == null)
            {
                return NotFound();
            }

            _context.Deck.Remove(deck);
            await _context.SaveChangesAsync();

            return deck;
        }

        private bool DeckExists(Guid id)
        {
            return _context.Deck.Any(e => e.DeckID == id);
        }
    }
}

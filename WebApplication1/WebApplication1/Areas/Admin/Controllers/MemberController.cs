using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FileNotFoundException = Business.Exceptions.FileNotFoundException;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MemberController : Controller
    {
        IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public IActionResult Index()
        {
            var members = _memberService.GetAll();
            return View(members);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Member member)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                _memberService.Add(member);
            }
            catch(FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(NullPhotoException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
           
            catch(ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(MemberNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _memberService.Delete(id);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound();
            }
            catch(MemberNotFoundException ex) 
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var member = _memberService.Get(x => x.Id == id);
            if (member == null) return NotFound();
            return View(member);
        }

        [HttpPost]
        public IActionResult Update(Member member)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _memberService.Update(member.Id,member);
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (MemberNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (MemberNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

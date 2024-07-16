using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using StudyTodocups.Data;
using StudyTodocups.ViewModels;
using System.Net.Mime;
using System.Net;
using StudyTodocups.Middleware;
using StudyTodocups.DAL;
using Microsoft.AspNetCore.Mvc.Filters;
using StudyTodocups.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StudyTodocups.Controllers
{
    [SiteAuthorize()]
    public class TestsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor httpContextAccessor;
        public TestsController(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor) 
        {
            _db = db;
            this.httpContextAccessor = httpContextAccessor;
        }
        public  IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Test(int id, string? errorMes, bool? isTestWrong)
        {
            List<QuestionVM> questionVMs = new List<QuestionVM>();
            var currentTest = await _db.Tests.Where(i=> i.Id == id).FirstOrDefaultAsync();
            if(currentTest == null)
            {
                return NotFound();
            }
            var questionList = await _db.Questions.Where(i=> i.Test_Id == currentTest.Id).ToListAsync();
            foreach(var question in questionList)
            {
                var posibleAnswers = await _db.PossibleAnswers.Where(i=> i.QuestId == question.Id).ToListAsync();
                QuestionVM questionVM = new QuestionVM()
                {
                    Name = question.Name,
                    Test_Id = question.Test_Id,
                    Id = question.Id,
                    Answers = posibleAnswers
                };
                questionVMs.Add(questionVM);
            }

            int? userId = httpContextAccessor.HttpContext?.Session.GetInt32("user_id");
            List<Answer> answersUsr = await _db.Answers.Where(i => i.UserId == userId && i.Test_id == id).ToListAsync();
            
            TestElemVM testElemVM = new TestElemVM()
            {
                Questions = questionVMs,
                Test_id = currentTest.Id,
                Answers = answersUsr,
                isTestWrong = isTestWrong
            };
            ViewBag.TestError = errorMes;
            //TestElemVM testElemVM = new TestElemVM()
            //{
            //    Question = _db.Questions.FirstOrDefault(),
            //    PossibleAnswers = _db.PossibleAnswers.ToList(),
            //};
            //
            ViewData["Title"] = "Тест - " + currentTest.Name;
            ViewData["NamePage"] = "Тест - " + currentTest.Name;
            List<string> breads = new List<string>();
            var moduleOfTest = await _db.Modules.Where(i => i.Id == currentTest.ModuleId).FirstOrDefaultAsync();
            breads.Add(moduleOfTest.Name);
            breads.Add("Тест");
            ViewBag.ModuleName = breads;
            return View(testElemVM);
        }
        [HttpPost]
        public async Task<JsonResult> AnswerQuest(AnswerVM answerVM)
        {

            int? userId = httpContextAccessor.HttpContext?.Session.GetInt32("user_id");
            var AnswerUsr = await _db.PossibleAnswers.Where(i=> i.Id == answerVM.Id).FirstOrDefaultAsync();

            if(AnswerUsr == null)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Answer doesnt exist", MediaTypeNames.Text.Plain);
            }
            var isHaveAnswer = await _db.Answers.Where(i => i.Quest_Id == answerVM.Quest_id && i.UserId == userId).FirstOrDefaultAsync();
            if (isHaveAnswer == null)
            {
                Answer answer = new Answer()
                {
                    UserId = userId,
                    IsCorrect = AnswerUsr.IsCorrect,
                    PossibleAnswerId = AnswerUsr.Id,
                    Quest_Id = AnswerUsr.QuestId,
                    Test_id = answerVM.Test_id
                    
                };
                _db.Answers.Add(answer);
            }
            else
            {
                isHaveAnswer.IsCorrect = AnswerUsr.IsCorrect;
                isHaveAnswer.PossibleAnswerId = AnswerUsr.Id;
            }
            await _db.SaveChangesAsync();           
            //Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //return Json("The attached file is not supported", MediaTypeNames.Text.Plain);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json("Message sent!");
        }
        [HttpPost]
        public async Task<IActionResult> CompleteTest(int test_id)
        {
            int? userId = httpContextAccessor.HttpContext?.Session.GetInt32("user_id");
            var AnswersUser = await _db.Answers.Where(i=> i.UserId == userId && i.Test_id == test_id && i.IsCorrect == true).CountAsync();
            var testQuestions = await _db.PossibleAnswers.Where(i=> i.Test_id == test_id && i.IsCorrect == true).CountAsync();
            var AnswersUserAll = await _db.Answers.Where(i => i.UserId == userId && i.Test_id == test_id).CountAsync();
            var posibAnwers = await _db.PossibleAnswers.Where(i => i.Test_id == test_id).CountAsync();
            //Если все ответы правильные
            if (testQuestions == AnswersUser)
            {
                var test = await _db.Tests.Where(i=> i.Id == test_id).FirstOrDefaultAsync();
                var currentTest = await _db.Tests.Where(i => i.Id == test_id).FirstOrDefaultAsync();
                var nextModule = await _db.Modules.Where(i=>i.Id > currentTest.ModuleId).Select(i=> i.Id).FirstOrDefaultAsync();
                TestTry testTry = new TestTry()
                {
                    TestId = test_id,
                    UserId = userId
                };
                _db.TestTries.Add(testTry);
                await _db.SaveChangesAsync();
                List<string> breads = new List<string>();
                var moduleOfTest = await _db.Modules.Where(i => i.Id == currentTest.ModuleId).FirstOrDefaultAsync();
                int lastTestId = await _db.Tests.OrderBy(i => i.Id).Select(i=> i.Id).LastOrDefaultAsync();              
                if(lastTestId == test_id)
                {
                    SuccessTestVM successTestVM = new SuccessTestVM()
                    {
                        NextModuleId = nextModule,
                        IsLastTest = true
                    };
                    breads.Add(moduleOfTest.Name);
                    breads.Add("Тест");
                    breads.Add("Пройден");
                    return View("SuccessTest", successTestVM);
                }
                else
                {
                    SuccessTestVM successTestVM = new SuccessTestVM()
                    {
                        NextModuleId = nextModule,
                        IsLastTest = false
                    };
                    breads.Add(moduleOfTest.Name);
                    breads.Add("Тест");
                    breads.Add("Пройден");
                    ViewBag.ModuleName = breads;
                    return View("SuccessTest", successTestVM);
                }
               
            }
            //Если есть не отвеченные вопросы
            else if (testQuestions != AnswersUserAll)
            {
                return RedirectToAction("Test", new { id = test_id, errorMes = "Ответьте на все вопросы!" });
            }
            //Если все вопросы отвечены но есть неправильные ответы
            else
            {
                int b = 6;
                return RedirectToAction("Test", new { id = test_id, errorMes = "Тест не пройден! Есть неправильные ответы", isTestWrong = true });
            }
           
        }
        public async Task<IActionResult> TestReplay(int id)
        {
            int? userId = httpContextAccessor.HttpContext?.Session.GetInt32("user_id");
            List<Answer> answers = await _db.Answers.Where(i=> i.Test_id == id && i.UserId == userId).ToListAsync();
            foreach(var p in answers)
            {
                _db.Answers.Remove(p);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Test", new {id = id});
        }
        }
}

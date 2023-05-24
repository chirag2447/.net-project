using communityWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;

namespace communityWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProjectContext _context;
        private readonly IWebHostEnvironment _environment;
        public HomeController(ProjectContext context, IWebHostEnvironment hostEnvironment)
        {
            _environment = hostEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            var user = HttpContext.Session.GetString("name");
            var uid = HttpContext.Session.GetInt32("Id");

            homeView v = new homeView();
            v.FriendRequests = _context.FriendRequests.Where(f => f.SenderId == uid).ToList();
            if (user != null)
            {

                var f = _context.FriendRequests.Where(f => f.Sender.Fname == user && f.Status == "Approved").Select(f => new
                {
                    name = f.Receiver.Fname
                }).ToList();
                foreach (var i in f)
                {
                    Console.WriteLine("+++++" + i.name);
                }
            }

            /* v.Posts = (from post in _context.Posts
                        join communityMember in _context.CommunityMembers on post.CommunityId equals communityMember.CommunityId
                        where communityMember.UserId == uid
                        select post).ToList();*/
            if (uid != null)
            {

              /*  v.Posts = _context.Posts
        .Where(p => _context.CommunityMembers
            .Where(cm => cm.UserId == uid)
            .Select(cm => cm.CommunityId)
            .Contains(p.CommunityId)).OrderByDescending(p => p.CreatedDate)
        .ToList();*/

                v.Posts = _context.Posts.Include(c => c.Community).Include(c => c.User).ToList();
              /*  var p = _context.Posts.Where(p => _context.CommunityMembers
            .Where(cm => _context.FriendRequests.Select(f=>f.ReceiverId).Contains(cm.UserId))
            .Select(cm => cm.CommunityId)
            .Contains(p.CommunityId)).OrderByDescending(p => p.CreatedDate)
        .ToList();*/

            }
            else
            {

                v.Posts = _context.Posts.Include(c => c.Community).Include(c => c.User).ToList();
            }

            v.Communities = _context.Communities.Include(c => c.Owner).ToList();
            if (user != null)
            {
                v.Community = _context.CommunityMembers.Include(u => u.User).Include(u => u.Community).Where(m => m.User.Fname.ToLower() == user.ToLower()).Select(s => s.Community).ToList();
            }

            foreach (var h in Request.Headers)
            {
                Console.WriteLine("----" + h);
            }

            return View(v);
        }
        public IActionResult Details(int id)
        {
            var user = HttpContext.Session.GetString("name");
            SinglePostView sp = new SinglePostView();
            var p = _context.Posts.Where(p => p.Id == id).Include(p => p.Community).Include(p => p.User).First();
            var c = _context.PostFeedbacks.Where(p => p.PostId == id).Include(p => p.User).ToList();
            var numberOfMemberInCommunity = _context.CommunityMembers.Where(s => s.Community.Name == p.Community.Name).Count();
            sp.Post = p;
            sp.Feedbacks = c;
            sp.noOfMembers = numberOfMemberInCommunity.ToString();
            if (user != null)
            {

                sp.userCommunity = _context.CommunityMembers.Where(m => m.User.Fname.ToLower() == user.ToLower()).Select(s => s.Community).ToList();
            }


            return View(sp);
        }

        public IActionResult Comment(string text, int id)
        {
            var user = HttpContext.Session.GetString("name");
            if (user != null)
            {

                var u = _context.Users.Where(u => u.Fname == user).First();
                int i = u.Id;

                PostFeedback postFeedback = new PostFeedback();
                postFeedback.UserId = i;
                postFeedback.PostId = id;
                postFeedback.Review = text;
                postFeedback.CreatedDate = DateTime.Now;
                _context.Add(postFeedback);
                _context.SaveChangesAsync();
                return RedirectToAction("details", new { id = id });
            }
            else
            {
                return RedirectToAction("Login", "register");
            }

        }
        public IActionResult Join(int communityId)
        {
            var user = HttpContext.Session.GetString("name");
            if (user == null)
            {
                return RedirectToAction("Login", "Register");
            }
            else
            {
                CommunityMember m = new CommunityMember();
                m.CommunityId = communityId;
                var u = _context.Users.Where(u => u.Fname == user).First();
                m.UserId = u.Id;
                _context.Add(m);
                _context.SaveChangesAsync();
                return Redirect(Request.Headers["Referer"].ToString());
            }

        }
        public IActionResult Leave(int communityId)
        {
            var user = HttpContext.Session.GetString("name");
            var Id = (int)HttpContext.Session.GetInt32("Id");
            if (user == null)
            {
                return RedirectToAction("Login", "Register");
            }
            else
            {
                CommunityMember m = _context.CommunityMembers.Where(c => c.CommunityId == communityId && c.UserId == Id).First();



                _context.CommunityMembers.Remove(m);
                _context.SaveChangesAsync();
                /* return RedirectToAction("communitydetail", new {id = communityId});*/
                return Redirect(Request.Headers["Referer"].ToString());
            }

        }

        public IActionResult SearchPost(string search)
        {
            var user = HttpContext.Session.GetString("name");
            HttpContext.Session.SetString("search", search);


            SearchPostView v = new SearchPostView();
            v.Posts = _context.Posts.Where(p => p.Title.ToLower().Contains(search.ToLower())).Include(c => c.Community).Include(c => c.User).ToList();
            v.Communities = _context.Communities.Where(c => c.Name.ToLower().Contains(search.ToLower())).Include(c => c.Owner).ToList();
            v.Feedbacks = _context.PostFeedbacks.ToList();
            v.Members = _context.CommunityMembers.ToList();
            if (user != null)
            {

                v.userCommunity = _context.CommunityMembers.Where(m => m.User.Fname.ToLower() == user.ToLower()).Select(s => s.Community).ToList();
            }



            return View(v);

        }
        public IActionResult searchCommunity()
        {
            var user = HttpContext.Session.GetString("name");
            var searchString = HttpContext.Session.GetString("search");
            SearchPostView v = new SearchPostView();
            v.Communities = _context.Communities.Where(c => c.Name.ToLower().Contains(searchString.ToLower())).Include(c => c.Owner).ToList();
            v.Members = _context.CommunityMembers.ToList();
            if (user != null)
            {

                v.userCommunity = _context.CommunityMembers.Where(m => m.User.Fname.ToLower() == user.ToLower()).Select(s => s.Community).ToList();
            }
            return View(v);
        }

        public IActionResult CommunityDetail(int id)
        {
            var user = HttpContext.Session.GetString("name");
            communityDetailView v = new communityDetailView();
            if (user != null)
            {
                var a = _context.CommunityMembers.Where(m => m.User.Fname == user && m.CommunityId == id).FirstOrDefault();
                if (a != null)
                {

                    v.IsMember = true;
                }
                else
                {
                    v.IsMember = false;
                }
            }
            else
            {
                v.IsMember = false;
            }
            v.Community = _context.Communities.Where(c => c.Id == id).First();
            v.Members = _context.CommunityMembers.Where(m => m.CommunityId == id).ToList();
            v.Posts = _context.Posts.Where(p => p.CommunityId == id).Include(p => p.User).ToList();
            return View(v);
        }
        public IActionResult UserDetail(int id)
        {
            var user = HttpContext.Session.GetString("name");
            userDetailView v = new userDetailView();
            v.User = _context.Users.Where(c => c.Id == id).First();
            ViewBag.user = v.User.Fname;
            var f = _context.FriendRequests.Where(f => f.ReceiverId == id && f.Status == "Approved").ToList();
            v.noOfFriends = f.Count();
            if (user != null)
            {
                var a = _context.CommunityMembers.Where(m => m.User.Fname == user && m.CommunityId == id).FirstOrDefault();
                v.IsMember = true;
                try
                {

                    v.Friend = _context.FriendRequests.Where(f => f.Sender.Fname == user && f.Receiver.Fname == v.User.Fname && f.Status == "Approved").First();
                }
                catch (Exception)
                {


                }
            }
            else
            {
                v.IsMember = false;
            }

            v.Posts = _context.Posts.Where(p => p.UserId == id).Include(p => p.User).Include(p => p.Community).ToList();
            return View(v);
        }

        public IActionResult Follow(int toFollowId)
        {
            if (HttpContext.Session.GetString("name") != null)
            {

                int UserId = (int)HttpContext.Session.GetInt32("Id");

                FriendRequest friendRequest = new FriendRequest();
                friendRequest.SenderId = UserId;
                friendRequest.ReceiverId = toFollowId;
                friendRequest.Status = "Approved";
                friendRequest.SentDate = DateTime.Now;
                _context.Add(friendRequest);
                _context.SaveChangesAsync();
                return RedirectToAction("userdetail", new { id = toFollowId });
            }
            else
            {
                return RedirectToAction("login", "register");
            }


        }
        public IActionResult UnFollow(int toUnFollowId)
        {
            if (HttpContext.Session.GetString("name") != null)
            {

                int UserId = (int)HttpContext.Session.GetInt32("Id");

                var existingFriend = _context.FriendRequests.Where(f => f.ReceiverId == toUnFollowId && f.SenderId == UserId).First();



                _context.FriendRequests.Remove(existingFriend);
                _context.SaveChangesAsync();
                return RedirectToAction("userdetail", new { id = toUnFollowId });
            }
            else
            {
                return RedirectToAction("login", "register");
            }


        }

        public async Task<IActionResult> AddCommunity(string community, string des, IFormFile file)
        {
            var id = HttpContext.Session.GetInt32("Id");
            if (id != null)
            {

                Community newcommunity = new Community();

                if (file != null)
                {
                    string filename = file.FileName;
                    string filepath = Path.Combine(_environment.WebRootPath, "img", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        file.CopyToAsync(stream);
                    }
                    newcommunity.Name = community;
                    newcommunity.Description = des;
                    newcommunity.Logo = filename;
                    newcommunity.IsApproved = true;
                    newcommunity.OwnerId = id;

                }
                newcommunity.CreatedDate = DateTime.Now;
                _context.Add(newcommunity);
                await _context.SaveChangesAsync();

                int i = getId(community);


                return RedirectToAction("communitydetail", new { id = i });
            }
            else
            {
                return RedirectToAction("login", "register");
            }



        }
        public int getId(string name)
        {
            var c = new Community();
            c = _context.Communities.Where(c => c.Name == name).FirstOrDefault();
            int id = c.Id;
            return id;
        }

        public IActionResult CreatePost(int cId)
        {
            if(cId != 0)
            {

            var community = _context.Communities.Where(c => c.Id == cId).First();
            ViewBag.cName = community.Name;
            }
            var id = HttpContext.Session.GetInt32("Id");
            if (id != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "register");
            }

        }
        public IActionResult SCommunity(string search)
        {
            var communities = _context.Communities.Where(c => c.Name.ToLower().Contains(search.ToLower())).ToList();

            return Json(communities);
        }
        public IActionResult SAllCommunity()
        {
            var communities = _context.Communities.ToList();

            return Json(communities);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(string title, string des, string community, IFormFile file)
        {
            var id = HttpContext.Session.GetInt32("Id");
            if (id != null)
            {
                Post post = new Post();
                if (file != null)
                {
                    string filename = file.FileName;
                    string filepath = Path.Combine(_environment.WebRootPath, "img", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {

                        await file.CopyToAsync(stream);
                    }
                    var c = _context.Communities.Where(c => c.Name.ToLower().Contains(community.ToLower())).First();

                    post.Title = title;
                    post.Description = des;
                    post.Type = "Public";
                    post.IsActive = true;
                    post.UserId = id;
                    post.CommunityId = c.Id;
                    post.FileName = filename;
                    post.CreatedDate = DateTime.Now;

                }
                _context.Add(post);
                await _context.SaveChangesAsync();
                int i = getPId(title);
                return RedirectToAction("details", new { id = i });
            }
            else
            {
                return RedirectToAction("login", "register");
            }

        }
        public int getPId(string name)
        {
            var p = _context.Posts.Where(p => p.Title == name).First();
            int i = p.Id;
            return i;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
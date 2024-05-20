using Exam.Bussiness.Exceptions;
using Exam.Bussiness.Service.Abstract;
using Exam.Core.Models;
using Exam.Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Bussiness.Service.Concretes
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TeamService( ITeamRepository repository,IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public void AddTeam(Team team)
        {
           if(team.ImgFile == null)
            {
                throw new ArgumentNullException("ImgFile","tapilmadi");
            }
            if (team.ImgFile.Length > 2097125)
            {
                throw new FileSizeException("(ImgFile", "olcu boyukdur");
            }
            if (!team.ImgFile.ContentType.Contains("image"))
            {
                throw new FileContentException("ImgFile", "Sekil tipi deyil");
            }
            string path=_webHostEnvironment.WebRootPath+ @"/Upload/Service/"+ team.ImgFile.FileName;
            using (FileStream stream=new FileStream(path, FileMode.Create))
            {
                team.ImgFile.CopyTo(stream);
            }
            team.ImgUrl = team.ImgFile.FileName;
            _repository.Add(team);
            _repository.Commit();

        }

        public List<Team> GetAllTeams(Func<Team, bool>? func = null)
        {
            return _repository.GetAll(func);
        }

        public Team GetTeam(Func<Team, bool>? func = null)
        {
            return _repository.Get(func);
        }

        public void RemoveTeam(int id)
        {
         var team=_repository.Get(x=>x.Id == id);
            if (team== null)
            {
                throw new Exception();
            }
            string path = _webHostEnvironment.WebRootPath + @"/Upload/Service/" + team.ImgUrl;
            if(!File.Exists(path))
            {
                throw new TeamNameNullReferanceException("ImgUrl", "File tapilmadi");
            }
       File.Delete(path);
     _repository.Remove(team);
            _repository.Commit() ;
        }

        public void UpdateTeam(int id, Team team)
        {
           var oldTeam= _repository.Get(x=> x.Id == id);
            if (oldTeam== null)
            {
                throw new NullReferenceException();
                
            }
            if(team.ImgFile!= null)
            {
                string filename = team.ImgFile.FileName;
                string path=_webHostEnvironment.WebRootPath +@"/Upload/Service/"+team.ImgFile.FileName;
                using(FileStream stream=new FileStream(path, FileMode.Create))
                {
                    team.ImgFile.CopyTo(stream);
                }
                FileInfo fileinfo = new FileInfo(path + oldTeam.ImgUrl);
                if (fileinfo.Exists)
                {
                    fileinfo.Delete();
                }
                oldTeam.ImgUrl = filename;
                
               
            }
            oldTeam.Name= team.Name;
            oldTeam.Description= team.Description;
            oldTeam.Position= team.Position;
            _repository.Commit() ;
        }
    }
}

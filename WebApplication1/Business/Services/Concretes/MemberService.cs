using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes
{
    public class MemberService:IMemberService
    {
        IMemberRepository _memberRepository;
        IWebHostEnvironment _environment;

        public MemberService(IMemberRepository memberRepository, IWebHostEnvironment environment = null)
        {
            _memberRepository = memberRepository;
            _environment = environment;
        }

        public void Add(Member member)
        {
            if (member == null) throw new MemberNullException("", "Member null ola bilmez");
            if (member.PhotoFile == null) throw new NullPhotoException("PhotoFile", "PhotoFile null ola bilmez");

            if (member.PhotoFile.Length > 2097152) throw new FileSizeException("", "Max olcu 2 mb ola biler!");
            if (!member.PhotoFile.ContentType.Contains("image/")) throw new ContentTypeException("PhotoFile", "Faylin tipi sehvdir");

            string path = _environment.WebRootPath + @"\uploads\" + member.PhotoFile.FileName;

            using(FileStream file = new FileStream(path, FileMode.Create))
            {
               member.PhotoFile.CopyTo(file);
            }

            member.ImgUrl = member.PhotoFile.FileName; 
            _memberRepository.Add(member);
            _memberRepository.Commit();


        }

        public void Delete(int id)
        {
            var existMember = _memberRepository.Get(x=>x.Id == id);
            if (existMember == null) throw new MemberNotFoundException("", "Member yoxdur!");

            string path = _environment.WebRootPath + @"\uploads\" + existMember.ImgUrl;

            if (!File.Exists(path)) throw new Exceptions.FileNotFoundException("", "File yoxdur");

            File.Delete(path);
            _memberRepository.Delete(existMember);
            _memberRepository.Commit();
        }

        public Member Get(Func<Member, bool> func = null)
        {
            return _memberRepository.Get(func);
        }

        public List<Member> GetAll(Func<Member, bool> func = null)
        {
            return _memberRepository.GetAll(func);
        }

        public void Update(int id, Member member)
        {
            if (member == null) throw new MemberNullException("", "Member null ola bilmez");
            var existMember = _memberRepository.Get(x => x.Id == id);
            if (existMember == null) throw new MemberNotFoundException("", "Member yoxdur!");
            if(member.PhotoFile != null)
            {
                if (member.PhotoFile.Length > 2097152) throw new FileSizeException("PhotoFile", "Max olu=cu 2 mb ola biler!");
                if (!member.PhotoFile.ContentType.Contains("image/")) throw new ContentTypeException("PhotoFile", "Faylin tipi sehvdir");

                string path = _environment.WebRootPath + @"\uploads\" + member.PhotoFile.FileName;

                using (FileStream file = new FileStream(path, FileMode.Create))
                {
                    member.PhotoFile.CopyTo(file);
                }

                existMember.ImgUrl = member.PhotoFile.FileName;
            }
            existMember.Fullname = member.Fullname;
            existMember.Position = member.Position;

            _memberRepository.Commit();
        }
    }
}

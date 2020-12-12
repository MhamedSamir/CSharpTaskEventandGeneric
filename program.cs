using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Day04AdvTasks
{
    class exam<T,tT> : Dictionary<T,tT>
    {

        public int id { get; set; }
        public int time { get; set; }
        public string subject { get; set; }
        public mode examMode { get; set; }
        public DateTime examEndTime { get; set; }
        public event EventHandler<getDate> examEvent;
        public event EventHandler<getDate> endExamEvent;

        public exam(int id, string subject, mode mode, int time)
        {
            this.id = id;
            this.subject = subject;
            this.examMode = mode;
            this.time = time;
            examEndTime = (new getDate().dt.AddMinutes(time));
           
            examEvent += staff.startMonitor;
            examEvent += student.startExam;
            examEvent += start;
            examEvent(this, new getDate());


        }


        public static void start(object o,getDate d)
        {
            exam<Question, ListAnswer> dic = o as exam<Question,ListAnswer>;

            Question q1 = new Question(1, "True Or False", "Can We ovrride static Method ", 3);
            Answer an1 = new Answer(1, "True");
            Answer an2 = new Answer(2, "False");
            ListAnswer lstAns1 = new ListAnswer();
            lstAns1.Add(an1);
            lstAns1.Add(an2);
            lstAns1.correctAnswer = an2.txt;

            Question q2 = new Question(2, "True Or False", "Can We ovrride sealed class", 2);
            ListAnswer lstAns2 = new ListAnswer();
            lstAns2.Add(an1);
            lstAns2.Add(an2);
            lstAns2.correctAnswer = an2.txt;

            Question q3 = new Question(3, "Choose correct Answer", "what is constructor use to decleare static members", 6);
            Answer ans3 = new Answer(1, "private constructor");
            Answer ans4 = new Answer(2, "public constructor");
            Answer ans5 = new Answer(3, "static constructor");
            Answer ans6 = new Answer(4, "All of above");
            ListAnswer lstAns3 = new ListAnswer();
            lstAns3.AddRange(new Answer[] { ans3, ans4, ans5, ans6 });
            lstAns3.correctAnswer = ans5.txt;

            Question q4 = new Question(4, "Choose Correct Anser", "How we Decleare New Object From class", 7);
            Answer ans7 = new Answer(1, "new className");
            Answer ans8 = new Answer(2, "new className()");
            Answer ans9 = new Answer(3, "className()");
            ListAnswer lstAns4 = new ListAnswer();
            lstAns4.AddRange(new Answer[] { ans7, ans8, ans9 });
            lstAns4.correctAnswer = ans8.txt;
            //================================================Dictionary
            
            dic.Add(q1, lstAns1);
            dic.Add(q2, lstAns2);
            dic.Add(q3, lstAns3);
            dic.Add(q4, lstAns4);
            int sum = 0;
            string answer;
            int Total = 0;
            string t = "";
            

            int c = 0;
            getDate dd=new getDate();
            foreach (var item in dic)
            {
                Console.WriteLine(item.Key.ToString());

                foreach (var ls in item.Value)
                {
                   
                         Console.WriteLine(ls);
                   
                   



                }
                Console.WriteLine("please Enter ID of Answer");
                if (dic.examEndTime <= DateTime.Now)
                {
                    dic.endExamEvent += student.endExam;
                    dic.endExamEvent += staff.endMonitor;
                    dic.endExamEvent += end;
                   
                    Console.WriteLine($"Your TotalMark is============> : {sum}");
                  
                    dd.dt = dic.examEndTime;
                    dic.endExamEvent(dic, dd);

                    return ;
                }
                    answer = Console.ReadLine();
                if (int.TryParse(answer, out c))
                    t = Program.getCorrect(item.Value, c);
                if (t == item.Value.correctAnswer || answer == item.Value.correctAnswer)
                {
                    sum += item.Key.mark;
                }

                Total += item.Key.mark;


            }
            //---------------------------------------get summtion of Total Grades--------------------

            Console.WriteLine("=====================================================");
            Console.WriteLine($"Your TotalMark is============> : {sum}/{Total}");
            Console.Write("=====================================================");
            //=================================================================answers===========

            dic.endExamEvent = student.endExam;
            dic.endExamEvent += end;
            dic.endExamEvent(dic, new getDate());
          

        }
        public static void end(object di,getDate d)
        {

            exam<Question, ListAnswer> dic = di as exam<Question, ListAnswer>;
            Console.WriteLine("======================A n s w e r s===================");

            foreach (var item in dic)
            {
                Console.WriteLine(item.Key.ToString());
                foreach (var v in item.Value)
                {
                    Console.WriteLine(v);


                }
                Console.WriteLine("CorrectAnswer >> " + item.Value.correctAnswer);

            }
        }
        public override string ToString()
        {
            return $"Exam Id{id},Subject{subject},ExamMode :{examMode} ,Time=({time} minutes),EndTime at({examEndTime})";

        }
    }
    class getDate : EventArgs
    {


        public DateTime dt = DateTime.Now;
    }
    enum mode
    {
        start, end
    }
    class student
    {

        public static void startExam(object o, getDate e)
        {
            exam <Question, ListAnswer > a = o as exam<Question, ListAnswer>;
            if (a != null)
                Console.WriteLine($"Students Start Exam: at {e.dt},\n{a.ToString()}");

        }
        public static void endExam(object o, getDate e)
        {
            exam<Question, ListAnswer> a = o as exam<Question, ListAnswer>;
            if (a != null)
                a.examMode = mode.end;
                Console.WriteLine($"Students Endes Exam: at {e.dt},\n{a.ToString()}");

        }
    }
    class staff
    {

        public static void startMonitor(object o, getDate e)
        {
            exam<Question, ListAnswer> a = o as exam<Question, ListAnswer>;
            if (a != null)
                Console.WriteLine($"Staff Started Monitoring Exam at {e.dt}\n{a.ToString()} ");

        }
        public static void endMonitor(object o, getDate e)
        {
            exam<Question, ListAnswer> a = o as exam<Question, ListAnswer>;
            if (a != null)
                a.examMode = mode.end;
                Console.WriteLine($"Staff Ended Monitoring Exam at {e.dt}\n{a.ToString()} ");

        }
    }


    class ListAnswer:List<Answer>
    {
        public string correctAnswer { get; set; }
        public override string ToString()
        {
            
            return base.ToString()+ $"CorrectAnswer:{correctAnswer}";
        }
    }
    public class Answer:ICloneable
    {
        public int id { get; set; }
        public string txt { get; set; }
        public Answer()
        {

        }
        public Answer(int id) : this()
        {
            this.id = id;
        }
        public Answer(int id, string txt) : this(id)
        {
            this.txt = txt;
        }
        public override string ToString()
        {
            return $"AnswerId:{id}:  {txt}";
        }

        public object Clone()
        {

            return new Answer() { id = this.id, txt = this.txt };
        }
    }

    class Question
    {
        public int id { get; set; }
        public string header { get; set; }
        public string body { get; set; }
        public int mark { get; set; }

        public Question()
        {

        }
        public Question(int id) : this()
        {
            this.id = id;

        }
        public Question(int id, string header) : this(id)
        {
            this.header = header;

        }
        public Question(int id, string header, string body) : this(id, header)
        {
            this.body = body;

        }
        public Question(int id, string header, string body, int mark) : this(id, header, body)
        {
            this.mark = mark;

        }
        public override string ToString()
        {
            return $"Id:({id}),Header:==>{header},Mark:{mark}\nBody:{body} ?";
        }

    }

  
    




    


      
    class Program
    {


    public static string getCorrect(ListAnswer lst,int a)
        {
            foreach (var item in lst)
            {
                if (a == item.id && item.txt==lst.correctAnswer )
                    return lst.correctAnswer;
            }
            return null;
        }
        static void Main(string[] args)
        {


            #region newBounsTaskDay05Event


            exam<Question, ListAnswer> e = new exam<Question, ListAnswer>(2, "oop", mode.start, 1);
            #endregion


          






            Console.ReadKey();
        }
    }
}

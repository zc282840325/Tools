using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace demo15
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }
        static List<Proxy> proxies = new List<Proxy>() { };

        public delegate void MethodDelegate(string url, string text, string xpath, string title, State state);
        private static MethodDelegate method;

        public object o = new object();

        static string name = "";

        static List<Model> models = new List<Model>()
        {

              new Model() {url= "https://s.weibo.com/user?Refer=weibo_user&q=", title= "--微博--", xpath= "//*[@id='pl_user_feedList']/div[1]/div[2]/p[3]/span[2]/a",state=State.Fan },
                new Model() {url= "https://www.so.com/s?q=", title= "--360--", xpath= "//*[@id='page']/span",state=State.Quantity },
             new Model() {url= "https://www.sogou.com/web?query=", title= "--搜狗--", xpath= "//*[@id='main']/div[1]/p",state=State.Quantity },
             new Model() {url= "https://news.so.com/ns?q=", title= "--360咨询--", xpath= "//*[@id='filter']/div[1]",state=State.Quantity },
             new Model() {url= "https://news.sogou.com/news?query=", title= "--搜狗新闻--", xpath= "//*[@id='wrapper']/div[1]/span[1]",state=State.Quantity },
            
             new Model() {url= "https://weixin.sogou.com/weixin?type=2&query=item", title= "--搜狗微信--", xpath= "//*[@id='pagebar_container']/div",state=State.Quantity },

          

             new Model() {url= "https://www.baidu.com/s?&word=", title= "--百度咨询--", xpath= "//*[@id='container']/div[2]/div/div[2]/span",state=State.Quantity },
             new Model() {url= "https://www.baidu.com/s?wd=", title= "--百度网页--", xpath= "//*[@id='container']/div[2]/div/div[2]/span",state=State.Quantity },

           new Model() {url= "https://baike.baidu.com/item/", title= "--百度百科--", xpath= "/html/body/div[3]/div[2]/div/div[3]/dl/dd[1]/ul/li[2]",state=State.Entry }
             //new Model() {url= "https://www.so.com/s?q=", title= "--360--", xpath= "//*[@id='page']/span",state=State.Quantity },
             //new Model() {url= "https://www.sogou.com/web?query=", title= "--搜狗--", xpath= "//*[@id='main']/div[1]/p",state=State.Quantity },
             //new Model() {url= "https://s.weibo.com/user?Refer=weibo_user&q=", title= "--微博--", xpath= "//*[@id='pl_user_feedList']/div[1]/div[2]/p[3]/span[2]/a",state=State.Fan }
              // new Model() {url= "https://baike.baidu.com/item/", title= "--百度百科--", xpath= "/html/body/div[3]/div[2]/div/div[2]/dl[2]/dd[1]/ul/li",state=State.Entry }
        };
        
        private void Button1_Click(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToString("hh:mm:ss");
             
            //List<string> list = new List<string>() {"海贼王","死神","火影","笔记本","鼠标","键盘","鼠标垫","腾讯" };
            List<string> list = File.ReadAllLines(Environment.CurrentDirectory + "//关键词100.txt", Encoding.UTF8).ToList();
            label3.Text = list.Count.ToString();


            System.Diagnostics.Stopwatch Watch1 = new System.Diagnostics.Stopwatch();
            Watch1.Start();
             AA(list,0);
            
                Watch1.Stop();
                var time = Watch1.ElapsedMilliseconds;
                label3.Text = time.ToString();
        }
        public void AA(List<string> list,int n)
        {
            if (n<=models.Count-1)
            {
            var task = new TaskFactory().StartNew(() =>
            {
                try
                {
                    Parallel.ForEach<string>(list, new ParallelOptions()
                    {
                        MaxDegreeOfParallelism = Environment.ProcessorCount - 1,
                    }, (item, loopState) =>
                    {
                        string urlsougou = models[n].url + item;
                        lock (o)
                        {
                            num++;
                            method = new MethodDelegate(GetManager);
                            method(urlsougou,models[n].xpath, models[n].title, item,models[n].state);
                        }

                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            });
















            task.ContinueWith(T => {
                //清除不可用的IP
                CheckIP();
                n++;
                AA(list,n);
            });
           }
        }


        public void GetManager(string url360, string xpath, string title, string text,State state)
        {
            try
            {

        
            HtmlWeb webClient = new HtmlWeb();
            webClient.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
            HtmlDocument doc1 = webClient.Load(url360);
            HtmlNodeCollection htmlNodes = null;
            HtmlNode node = null;
                //*[@id="pl_user_feedList"]/div[1]/div[2]/p[2]/span[2]/a
                node = doc1.DocumentNode.SelectSingleNode(xpath);

                while (node == null)
                {
                    Proxy ip = new Proxy();
                    try
                    {
                        HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(url360);

                        httpRequest = SetHttpWebRequest(httpRequest, ref ip);

                        using (HttpWebResponse rs = (HttpWebResponse)httpRequest.GetResponse())
                        {
                            using (System.IO.StreamReader sr = new StreamReader(rs.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8")))
                            {
                                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                doc.Load(sr);
                                node = doc.DocumentNode.SelectSingleNode(xpath);
                                if (node == null)
                                {
                                    ChangeState(ip);
                                    Gettip();
                                    continue;
                                }
                                else
                                {
                                    name = "启用代理IP";
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ChangeState(ip);
                        Gettip();
                        this.Invoke(new EventHandler(delegate
                        {
                            textBox1.AppendText(num + title + name + text + ":" + ex.Message + "\r\n");
                        }));
                        continue;
                    }

                
            }
          


            if (state== State.Quantity)
            {
                label1.Text = num.ToString();
                this.Invoke(new EventHandler(delegate
                {
                    textBox1.AppendText(num + title + name + text + ":" + GetNumber(node.InnerHtml) + "\r\n");
                }));
            }
            else if (state==State.Fan)
            {
                label1.Text = num.ToString();
                this.Invoke(new EventHandler(delegate
                {
                    textBox1.AppendText(num + title + name + text + ":" + PrintNumber(node.InnerText) + "\r\n");
                }));
            }
            else if (state==State.Entry)
            {
               
                label1.Text = num.ToString();
                this.Invoke(new EventHandler(delegate
                {
                    textBox1.AppendText(num + title + name + text + ":" + htmlNodes.Count + "\r\n");
                }));
            }
            else
            {
                this.Invoke(new EventHandler(delegate
                {
                    textBox1.AppendText("信息有问题\r\n");
                }));
            }

            }
            catch (Exception ex)
            {

                this.Invoke(new EventHandler(delegate
                {
                    textBox1.AppendText(ex.Message+"\r\n");
                })); 
            }

        }
        public string PrintNumber(string str)
        {
            double num =Convert.ToInt32(GetNumber(str));
            if (str.IndexOf("万")>0)
            {
                num = num * 10000;
            }
            else if (str.IndexOf("亿") > 0)
            {
                num = num * 10000*10000;
            }
           
            return num.ToString();
        }
        public static int num = 0;  

        private HttpWebRequest SetHttpWebRequest(HttpWebRequest httpRequest,ref Proxy ip)
        {
            httpRequest.Method = "Post";
            httpRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.108 Safari/537.36";
            httpRequest.KeepAlive = false;
            httpRequest.ProtocolVersion = HttpVersion.Version10;
            httpRequest.Timeout = 50000;
            if (proxies.Count == 0)
            {
                Gettip();
            }
            Proxy proxy1 = GetRandIP();
            WebProxy proxy = new WebProxy(proxy1.IP, proxy1.Port);//str为IP地址 port为端口号
            httpRequest.Proxy = proxy;
            ip = proxy1;
            return httpRequest;
        }

        #region 设置代理ip
        public void Gettip()
        {
            HtmlWeb webClient = new HtmlWeb();
            HtmlDocument doc1 = webClient.Load("http://http.tiqu.alicdns.com/getip3?num=1&type=2&pro=&city=0&yys=0&port=1&pack=56457&ts=0&ys=0&cs=0&lb=1&sb=0&pb=4&mr=1&regions=&gm=4");
            JObject jObject = (JObject)JsonConvert.DeserializeObject(doc1.Text);
            if (bool.Parse(jObject["success"].ToString()))
            {
                proxies.Add(new Proxy() { IP = jObject["data"][0]["ip"].ToString(), Port = Convert.ToInt32(jObject["data"][0]["port"].ToString()), State = 0 });
            }
            else
            {
                Console.WriteLine("");
            }

        }
        #endregion

        #region 正则取字符串中的数字
        public string GetNumber(string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, @"[^0-9]+", "");
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
        }

       
        #region 随机取代理IP
        public Proxy GetRandIP()
        {
            var list = proxies.Where(o => o.State == 0).ToList();
            if (list.Count >0)
            {
                int count = list.Count;
                Random random = new Random();
                int ran = random.Next(count);
                return list[ran];
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 修改代理IP的状态
        public void ChangeState(Proxy ip)
        {
            if (proxies.Count>0)
            {
                Proxy mm = proxies.Where(o => o == ip).SingleOrDefault();
                mm.State = 1;
                int index = proxies.FindIndex(item => item.IP.Equals(ip));
                proxies[index] = mm;
            }
        }
        #endregion

        #region 检查代理IP是否可用
        public void CheckIP()
        {
         
            for (int i = 0; i < proxies.Count; i++)
            {
                try
                {
                    Proxy proxy = proxies[i];
                    //1.0 创建Sock对象
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    //2.0 创建IP对象
                    IPAddress address = IPAddress.Parse(proxy.IP);
                    //3.0 创建网络端口,包括ip和端口
                    IPEndPoint endPoint = new IPEndPoint(address, proxy.Port);
                    //4.0 绑定套接字
                    socket.Bind(endPoint);
                    if (!socket.Connected)
                    {
                        proxies.Remove(proxies[i]);
                    }
                    else
                    {
                        proxies[i] = new Proxy(proxy.IP,proxy.Port,0);
                    }
                }
                catch (IOException e)
                {
                    
                }
            }
        }
        #endregion

        private void Button2_Click(object sender, EventArgs e)
        {
            HtmlNode node = null;
            List<string> list = new List<string>() { "海贼王", "死神", "火影", "笔记本", "鼠标", "键盘", "鼠标垫", "腾讯" };
            for (int i = 0; i < list.Count; i++)
            {
                string url360 = "https://baike.baidu.com/item/" + list[i];
                HtmlWeb webClient = new HtmlWeb();
                webClient.CaptureRedirect = true;
                HtmlDocument doc1 = webClient.Load(url360);
                node = doc1.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div/div[2]/dl[2]/dd[1]/ul/li");
                this.Invoke(new EventHandler(delegate
                {
                    //if (node==null)
                    //{
                    //    textBox1.AppendText(i + list[i] + ":" + 1 + "\r\n");
                    //}
                    //else
                    //{
                    //    textBox1.AppendText(i + list[i] + ":" + node.Count + "\r\n");
                    //}
                
                  textBox1.AppendText(num + list[i] + ":" + node.InnerText + "\r\n");
                }));
            }
           
        }
    }
}

# Tools
# 针对各大型搜索引擎的搜索结果爬虫 

    `HttpWebRequest and HtmlWeb`：发送请求
    `HtmlAgilityPack`：加载网页
    `Xpath`:语法取值
    `Parallel.ForEach`：多线程并发
    `WebProxy`：代理IP
    
## 微博(人气抓取)
    微博地址：'https://s.weibo.com/user?Refer=weibo_user&q='
    Xpath://*[@id='pl_user_feedList']/div[1]/div[2]/p[3]/span[2]/a
## 360(搜索结果)
    360搜索地址：https://www.so.com/s?q=
    Xpath://*[@id='page']/span
## 360咨询(搜索结果)
    360咨询搜索地址：https://news.so.com/ns?q=
    Xpath://*[@id='filter']/div[1]
## 搜狗(搜索结果)
    搜狗搜索地址：https://www.sogou.com/web?query=
    Xpath://*[@id='main']/div[1]/p
## 搜狗新闻(搜索结果)
    搜狗新闻搜索地址：https://news.sogou.com/news?query=
    Xpath://*[@id='wrapper']/div[1]/span[1]
## 百度(搜索结果)
    百度搜索地址：https://www.baidu.com/s?wd=
    Xpath://*[@id='container']/div[2]/div/div[2]/span
## 百度咨询(搜索结果)
    百度咨询搜索地址：https://www.baidu.com/s?&word=
    Xpath://*[@id='container']/div[2]/div/div[2]/span
## 百度百科(搜索结果)
    百度百科搜索地址：https://baike.baidu.com/item/
    Xpath:/html/body/div[3]/div[2]/div/div[3]/dl/dd[1]/ul/li[2]
    

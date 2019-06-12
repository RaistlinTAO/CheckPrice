// ######################################################################################################################
// #  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the  #
// #  following conditions are met:                                                                                     #
// #    1、Redistributions of source code must retain the above copyright notice, this list of conditions and the       #
// #       following disclaimer.                                                                                        #
// #    2、Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the    #
// #       following disclaimer in the documentation and/or other materials provided with the distribution.             #
// #    3、Neither the name of the D.E.M.O.N, K9998(Wei Tao) nor the names of its contributors may be used to endorse   #
// #       or promote products derived from this software without specific prior written permission.                    #
// #                                                                                                                    #
// #       Project Name:                                                                                                #
// #       Module  Name:                                                                                                #
// #       Part of:                                                                                                     #
// #       Date:                                                                                                        #
// #       Version:                                                                                                     #
// #                                                                                                                    #
// #                                           Copyright © 2011 ORG: D.E.M.O.N K9998(Wei Tao) All Rights Reserved      #
// ######################################################################################################################
namespace CheckPrice
{
    #region

    using System;
    using System.IO;
    using MySql.Data.MySqlClient;

    #endregion

    internal class Program
    {
        private static void Main()
        {
            //这里设定args的值为
            //保存路径
            try
            {
                var iReturnItem = new LXPrice[1000];
                string myCountQuery = "SELECT COUNT(*) FROM ecs_goods WHERE is_delete = 0;";
                string mySelectQuery =
                    "SELECT goods_id ,brand_id,goods_short_name ,goods_equip , goods_img ,goods_name ,goods_desc  FROM ecs_goods WHERE is_delete = 0 ORDER BY  brand_id ;";
                var iConnection =
                    new MySqlConnection(
                        "Server=127.0.0.1;Uid=root;Pwd=;Database=skymobile;Encrypt=true;Compress=true;");

                var myCommand = new MySqlCommand(myCountQuery, iConnection);

                iConnection.Open();
                MySqlDataReader myReader;
                myReader = myCommand.ExecuteReader();


                while (myReader.Read())
                {
                    //商品总数
                    iReturnItem = new LXPrice[myReader.GetInt16(0)];
                }
                myReader.Close();

                myCommand.CommandText = mySelectQuery;

                MySqlDataReader myReaderName = myCommand.ExecuteReader();

                int i = 0;
                while (myReaderName.Read())
                {
                    //商品总数
                    iReturnItem[i].iID = myReaderName.GetString(0);
                    iReturnItem[i].iBrand = myReaderName.GetString(1);
                    iReturnItem[i].iName = myReaderName.GetString(2);
                    iReturnItem[i].iEquip = myReaderName.GetString(3);
                    iReturnItem[i].iImg = "http://www.skymobile.com.cn/shop/" + myReaderName.GetString(4);
                    iReturnItem[i].iFullName = myReaderName.GetString(5);
                    try
                    {
                        iReturnItem[i].iDES = myReaderName.GetString(6);
                    }
                    catch (Exception)
                    {
                        iReturnItem[i].iDES = "";
                    }

                    iReturnItem[i].iURL = "http://www.skymobile.com.cn/shop/goods.php?id=" + iReturnItem[i].iID;
                    iReturnItem[i].iCP = GetCP(iReturnItem[i].iID);
                    Console.WriteLine("Got Value: ID = " + iReturnItem[i].iID + " "
                                      + iReturnItem[i].iName);
                    i++;
                }
                myReaderName.Close();

                iConnection.Close();
                iConnection.Dispose();

                //生成HTML格式文件
                if (MakeHTML(iReturnItem, @"D:\xampp\htdocs"))
                {
                    Console.WriteLine("MAKE WEBPAGE OK!");
                }
                else
                {
                    Console.WriteLine("MAKE WEBPAGE FAIL!");
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        private static bool MakeHTML(LXPrice[] iPrice, string Path)
        {
            #region 老CSS

            /*
            #region 定义大局样式表

            iTemp = iTemp + "<style type=\"text/css\">" +
                    "body{" +
                    "margin: 6px;" +
                    "padding: 0;" +
                    "font-size: 12px;" +
                    "font-family: tahoma, arial;" +
                    "background: #fff;" +
                    "}" +
                    "table{" +
                    "width: 100%;" +
                    "}" +
                    "tr.odd{" +
                    "background: #fff;" +
                    "}" +
                    "tr.even{" +
                    "background: #eee;" +
                    "}" +
                    "div.datagrid_div{" +
                    "width: 100%;" +
                    "border: 1px solid #999;" +
                    "}";

            #endregion

            #region 定义表格样式表

            iTemp = iTemp + "table.datagrid2{" +
                    "border-collapse: collapse; " +
                    "table.datagrid2{" +
                    "border-collapse: collapse; " +
                    "}" +
                    "table.datagrid2 th{" +
                    "text-align: left;" +
                    "background: #9cf;" +
                    "padding: 3px;" +
                    "border: 1px #333 solid;" +
                    "}" +
                    "table.datagrid2 td{" +
                    "padding: 3px;" +
                    "border: none;" +
                    "border:1px #333 solid;" +
                    "}" +
                    "tr:hover," +
                    "tr.hover{" +
                    "background: #9cf;" +
                    "}" +
                    "</style>";

            #endregion

            #region Java script

            iTemp = iTemp + "<script type=\"text/javascript\">" +
                    "function add_event(tr){" +
                    "tr.onmouseover = function(){" +
                    "tr.className += ' hover';" +
                    "};" +
                    "tr.onmouseout = function(){" +
                    "tr.className = tr.className.replace(' hover', '');" +
                    "};" +
                    "}" +
                    "function stripe(table) {" +
                    "var trs = table.getElementsByTagName(\"tr\");" +
                    "for(var i=1; i<trs.length; i++){" +
                    "var tr = trs[i];" +
                    "tr.className = i%2 != 0? 'odd' : 'even';" +
                    "add_event(tr);" +
                    "}" +
                    "}" +
                    "window.onload = function(){" +
                    "var tables = document.getElementsByTagName('table');" +
                    "for(var i=0; i<tables.length; i++){" +
                    "var table = tables[i];" +
                    "if(table.className == 'datagrid1' || table.className == 'datagrid2'" +
                    "|| table.className == 'datagrid3' || table.className == 'datagrid4'){" +
                    "stripe(tables[i]);" +
                    "}" +
                    "}" +
                    "}" +
                    "</script>";
                   

            #endregion
            */

            #endregion

            #region 老生成代码(1页面1div)

            /*
            iTemp = iTemp +
                    "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /><meta http-equiv=\"Content-Language\" content=\"zh-cn\" />";
            iTemp = iTemp +
                    "<style type=\"text/css\">body { font: normal 11px auto \"Trebuchet MS\", Verdana, Arial, Helvetica, sans-serif; color: #4f6b72; background: #E6EAE9; } a {color: #c75f3e; }#mytable { width: 950px;padding: 0;margin: 0;}caption { padding: 0 0 5px 0; width: 700px; font: italic 11px \"Trebuchet MS\", Verdana, Arial, Helvetica, sans-serif; text-align: right; }th {font: bold 11px \"Trebuchet MS\", Verdana, Arial, Helvetica, sans-serif; color: #4f6b72; border-right: 1px solid #C1DAD7; border-bottom: 1px solid #C1DAD7; border-top: 1px solid #C1DAD7; letter-spacing: 2px; text-transform: uppercase; text-align: left; padding: 6px 6px 6px 12px; background: #CAE8EA  no-repeat; } th.nobg { border-top: 0; border-left: 0; border-right: 1px solid #C1DAD7; background: none; } td { border-right: 1px solid #C1DAD7; border-bottom: 1px solid #C1DAD7; background: #fff; font-size:11px; padding: 6px 6px 6px 12px; color: #4f6b72; } td.alt { background: #F5FAFA; color: #797268; } th.spec { border-left: 1px solid #C1DAD7; border-top: 0; background: #fff no-repeat; font: bold 10px \"Trebuchet MS\", Verdana, Arial, Helvetica, sans-serif; }th.specalt { border-left: 1px solid #C1DAD7; border-top: 0; background: #f5fafa no-repeat; font: bold 10px \"Trebuchet MS\", Verdana, Arial, Helvetica, sans-serif; color: #797268; } /*---------for IE 5.x bug♥1♥ html>body td{ font-size:11px;} body,td,th { font-family: 宋体, Arial; font-size: 12px; } </style>";

            iTemp = iTemp + "</head>" +
                    "<body>";
            iTemp = iTemp + "<center><H2>龙翔通讯每日报价</H2></center><br>";
            iTemp = iTemp + "<center><H5>最后生成时间" + DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day +
                    "日" + DateTime.Now.Hour + "时" + "</H5></center><br><br>";

            iTemp = iTemp + "<center>诺基亚 Nokia</center>" +
                    "<div>" +
                    "<table align = 'center' width='1000'>" +
                    "<tr>" +
                    "<th>手机名称</th>" +
                    "<th>手机类型</th>" +
                    "<th>手机价格</th>" +
                    "<th>手机配件</th>" +
                    "<th>查看详细</th>" +
                    "</tr>";

            for (int i = 0; i < iPrice.Length; i++)
            {
                if (iPrice[i].iBrand == "1")
                {
                    for (int j = 0; j < iPrice[i].iCP.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(iPrice[i].iCP[j].iColor))
                        {
                            iTemp = iTemp + "<tr>" +
                                    "<td>" + iPrice[i].iName + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iColor + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iPrice + "</td>" +
                                    "<td>" + iPrice[i].iEquip + "</td>" +
                                    "<td><a href =\"" + iPrice[i].iURL + "\" target = \"_blank\">详情</a></td>";
                        }
                    }
                }
            }

            iTemp = iTemp + "</table>" +
                    "</div>";

            ////////////////////

            iTemp = iTemp + "<center>苹果 iPhone</center>" +
                    "<div>" +
                    "<table align = 'center' width='1000'>" +
                    "<tr>" +
                    "<th>手机名称</th>" +
                    "<th>手机类型</th>" +
                    "<th>手机价格</th>" +
                    "<th>手机配件</th>" +
                    "<th>查看详细</th>" +
                    "</tr>";

            for (int i = 0; i < iPrice.Length; i++)
            {
                if (iPrice[i].iBrand == "2")
                {
                    for (int j = 0; j < iPrice[i].iCP.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(iPrice[i].iCP[j].iColor))
                        {
                            iTemp = iTemp + "<tr>" +
                                    "<td>" + iPrice[i].iName + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iColor + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iPrice + "</td>" +
                                    "<td>" + iPrice[i].iEquip + "</td>" +
                                    "<td><a href =\"" + iPrice[i].iURL + "\" target = \"_blank\">详情</a></td>";
                        }
                    }
                }
            }

            iTemp = iTemp + "</table>" +
                    "</div>";

            ///////////////////////////////////////
            iTemp = iTemp + "<center>多普达 HTC</center>" +
                    "<div>" +
                    "<table align = 'center' width='1000'>" +
                    "<tr>" +
                    "<th>手机名称</th>" +
                    "<th>手机类型</th>" +
                    "<th>手机价格</th>" +
                    "<th>手机配件</th>" +
                    "<th>查看详细</th>" +
                    "</tr>";

            for (int i = 0; i < iPrice.Length; i++)
            {
                if (iPrice[i].iBrand == "3")
                {
                    for (int j = 0; j < iPrice[i].iCP.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(iPrice[i].iCP[j].iColor))
                        {
                            iTemp = iTemp + "<tr>" +
                                    "<td>" + iPrice[i].iName + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iColor + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iPrice + "</td>" +
                                    "<td>" + iPrice[i].iEquip + "</td>" +
                                    "<td><a href =\"" + iPrice[i].iURL + "\" target = \"_blank\">详情</a></td>";
                        }
                    }
                }
            }

            iTemp = iTemp + "</table>" +
                    "</div>";

            /////////////////////////////////
            iTemp = iTemp + "<center>三星 Samsung</center>" +
                    "<div>" +
                    "<table align = 'center' width='1000'>" +
                    "<tr>" +
                    "<th>手机名称</th>" +
                    "<th>手机类型</th>" +
                    "<th>手机价格</th>" +
                    "<th>手机配件</th>" +
                    "<th>查看详细</th>" +
                    "</tr>";

            for (int i = 0; i < iPrice.Length; i++)
            {
                if (iPrice[i].iBrand == "4")
                {
                    for (int j = 0; j < iPrice[i].iCP.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(iPrice[i].iCP[j].iColor))
                        {
                            iTemp = iTemp + "<tr>" +
                                    "<td>" + iPrice[i].iName + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iColor + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iPrice + "</td>" +
                                    "<td>" + iPrice[i].iEquip + "</td>" +
                                    "<td><a href =\"" + iPrice[i].iURL + "\" target = \"_blank\">详情</a></td>";
                        }
                    }
                }
            }

            iTemp = iTemp + "</table>" +
                    "</div>";

            ////////////////////////////////

            iTemp = iTemp + "<center>索爱 SonyEricsson</center>" +
                    "<div>" +
                    "<table align = 'center' width='1000'>" +
                    "<tr>" +
                    "<th>手机名称</th>" +
                    "<th>手机类型</th>" +
                    "<th>手机价格</th>" +
                    "<th>手机配件</th>" +
                    "<th>查看详细</th>" +
                    "</tr>";

            for (int i = 0; i < iPrice.Length; i++)
            {
                if (iPrice[i].iBrand == "5")
                {
                    for (int j = 0; j < iPrice[i].iCP.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(iPrice[i].iCP[j].iColor))
                        {
                            iTemp = iTemp + "<tr>" +
                                    "<td>" + iPrice[i].iName + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iColor + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iPrice + "</td>" +
                                    "<td>" + iPrice[i].iEquip + "</td>" +
                                    "<td><a href =\"" + iPrice[i].iURL + "\" target = \"_blank\">详情</a></td>";
                        }
                    }
                }
            }

            iTemp = iTemp + "</table>" +
                    "</div>";


            ///////////////////////////////////

            iTemp = iTemp + "<center>摩托罗拉 Motorola</center>" +
                    "<div>" +
                    "<table align = 'center' width='1000'>" +
                    "<tr>" +
                    "<th>手机名称</th>" +
                    "<th>手机类型</th>" +
                    "<th>手机价格</th>" +
                    "<th>手机配件</th>" +
                    "<th>查看详细</th>" +
                    "</tr>";

            for (int i = 0; i < iPrice.Length; i++)
            {
                if (iPrice[i].iBrand == "6")
                {
                    for (int j = 0; j < iPrice[i].iCP.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(iPrice[i].iCP[j].iColor))
                        {
                            iTemp = iTemp + "<tr>" +
                                    "<td>" + iPrice[i].iName + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iColor + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iPrice + "</td>" +
                                    "<td>" + iPrice[i].iEquip + "</td>" +
                                    "<td><a href =\"" + iPrice[i].iURL + "\" target = \"_blank\">详情</a></td>";
                        }
                    }
                }
            }

            iTemp = iTemp + "</table>" +
                    "</div>";

            ///////////////////////////////////

            iTemp = iTemp + "<center>LG</center>" +
                    "<div>" +
                    "<table align = 'center' width='1000'>" +
                    "<tr>" +
                    "<th>手机名称</th>" +
                    "<th>手机类型</th>" +
                    "<th>手机价格</th>" +
                    "<th>手机配件</th>" +
                    "<th>查看详细</th>" +
                    "</tr>";

            for (int i = 0; i < iPrice.Length; i++)
            {
                if (iPrice[i].iBrand == "7")
                {
                    for (int j = 0; j < iPrice[i].iCP.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(iPrice[i].iCP[j].iColor))
                        {
                            iTemp = iTemp + "<tr>" +
                                    "<td>" + iPrice[i].iName + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iColor + "</td>" +
                                    "<td>" + iPrice[i].iCP[j].iPrice + "</td>" +
                                    "<td>" + iPrice[i].iEquip + "</td>" +
                                    "<td><a href =\"" + iPrice[i].iURL + "\" target = \"_blank\">详情</a></td>";
                        }
                    }
                }
            }

            iTemp = iTemp + "</table>" +
                    "</div>";

            iTemp = iTemp +
                    "<div><p><h4>更多机型报价请访问:<a href = \"http://www.skymobile.com.cn/shop\">http://www.skymobile.com.cn/shop</a></h4></p>" +
                    "</div>";
            //////////////////////////////////
            /// 
            iTemp = iTemp + "<script type=\"text/javascript\"><!--" +
                    "google_ad_client = \"ca-pub-5853993113305574\";" +
                    "/* 龙翔通讯 ♥1♥" +
                    "google_ad_slot = \"9099981090\";" +
                    "google_ad_width = 728;" +
                    "google_ad_height = 90;" +
                    "//-->" +
                    "</script>" +
                    "<script type=\"text/javascript\"" +
                    "src=\"http://pagead2.googlesyndication.com/pagead/show_ads.js\">" +
                    "</script>";
            iTemp = iTemp + "</body>" +
                    "</html>";
*/

            #endregion

            #region 1.2版本页面 HEAD

            string iTemp =
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n";

            iTemp = iTemp + "<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n";
            iTemp = iTemp + "<head>\r\n";
            iTemp = iTemp + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />";
            iTemp = iTemp + "<title>龙翔通讯实时手机报价</title>\r\n";
            iTemp = iTemp + "<meta name=\"keywords\" content=\"手机报价 \" />\r\n";
            iTemp = iTemp + "<meta name=\"description\" content=\"手机报价 \" />\r\n";
            //<link rel="stylesheet" rev="stylesheet" href="{S_URL}/templates/$_SCONFIG[template]/image/list.css" type="text/css" media="all" />
            //iTemp = iTemp +
            //        "<link rel=\"stylesheet\" type=\"text/css\" href=\"http://www.skymobile.com.cn/list.css\" />\r\n";
            iTemp = iTemp +
                    "<link rel=\"stylesheet\" type=\"text/css\" href=\"http://www.skymobile.com.cn/templates/default/css/common.css\" />\r\n";
            iTemp = iTemp +
                    "<script type=\"text/javascript\" src=\"http://www.skymobile.com.cn/templates/default/js/common.js\"></script>\r\n";
            iTemp = iTemp + "<script type=\"text/javascript\">\r\n";
            iTemp = iTemp + "var siteUrl = \"http://www.skymobile.com.cn\";\r\n";
            iTemp = iTemp + "</script>\r\n";
            iTemp = iTemp +
                    "<script src=\"http://www.skymobile.com.cn/include/js/menu.js\" type=\"text/javascript\" language=\"javascript\"></script>\r\n";
            iTemp = iTemp +
                    "<script src=\"http://www.skymobile.com.cn/include/js/ajax.js\" type=\"text/javascript\" language=\"javascript\"></script>\r\n";
            iTemp = iTemp +
                    "<script src=\"http://www.skymobile.com.cn/include/js/common.js\" type=\"text/javascript\" language=\"javascript\"></script>\r\n";

            iTemp = iTemp + "</head>\r\n";

            #endregion

            #region 1.2版本页面 BODY

            iTemp = iTemp + "<body>\r\n";
            iTemp = iTemp + "<div id=\"append_parent\"></div>\r\n";
            iTemp = iTemp + "<div id=\"ajaxwaitid\"></div>\r\n";
            iTemp = iTemp + "<div id=\"header\">\r\n";
            iTemp = iTemp + "<div>\r\n";
            iTemp = iTemp +
                    "<h1><a href=\"http://www.skymobile.com.cn\"><img src=\"http://www.skymobile.com.cn/images/xl_logo.jpg\" alt=\"龙翔通讯\" /></a></h1><div class=\"ad_header\"><img src=\"http://www.skymobile.com.cn/images/xl_ad.jpg\" alt=\"龙翔通讯\" /></div>\r\n";
            iTemp = iTemp + "</div> \r\n";
            iTemp = iTemp + "</div>\r\n";
            iTemp = iTemp + "<div id=\"nav\">\r\n";
            iTemp = iTemp + "<div class=\"main_nav\">\r\n";
            iTemp = iTemp + "<ul>\r\n";
            iTemp = iTemp + "<li><a href=\"http://www.skymobile.com.cn/index.php/action-news\">首页</a></li>";
            iTemp = iTemp + "<li><a href=\"http://www.skymobile.com.cn/shop\">手机商城</a></li>\r\n";
            iTemp = iTemp + "<li><a href=\"http://www.skymobile.com.cn/m.php?name=service\">龙翔保修</a></li>\r\n";
            iTemp = iTemp + "<li><a href=\"http://www.skymobile.com.cn/m.php?name=qaa\">用户咨询</a></li>\r\n";
            iTemp = iTemp + "<li><a href=\"http://www.skymobile.com.cn/m.php?name=secondhand\">二手机</a></li>\r\n";
            iTemp = iTemp + "<li><a href=\"http://www.skymobile.com.cn/m.php?name=rank\">积分换购</a></li>\r\n";
            iTemp = iTemp +
                    "<li><a href=\"http://www.skymobile.com.cn/index.php/action-channel-name-dc\">龙翔心得</a></li>\r\n";
            iTemp = iTemp + "<li><a href=\"http://www.skymobile.com.cn/m.php?name=course\">手机教程</a></li>\r\n";
            iTemp = iTemp + "<li><a href=\"http://www.skymobile.com.cn/m.php?name=download\">手机软件</a></li>\r\n";
            iTemp = iTemp + "<li><a href=\"http://www.skymobile.com.cn/index.php/category-72\">壁纸</a></li>\r\n";
            iTemp = iTemp +
                    "<li><a href=\"http://www.skymobile.com.cn/index.php/action-channel-name-about\">关于龙翔</a></li>\r\n";
            iTemp = iTemp + "</ul>\r\n";
            iTemp = iTemp + "</div>\r\n";
            iTemp = iTemp + "</div>\r\n";

           

            iTemp = iTemp + "<div align = center>\r\n";
            iTemp = iTemp + "<br>\r\n";
            iTemp = iTemp + "<p align = center><H5>最后生成时间" + DateTime.Now.Year + "年" + DateTime.Now.Month + "月" +
                    DateTime.Now.Day +
                    "日" + DateTime.Now.Hour + "时" + "</H5></p>\r\n";
            iTemp = iTemp + "<br>\r\n";
            iTemp = iTemp + "</div>\r\n";

            iTemp = iTemp + "<div class=\"column\">\r\n";
            iTemp = iTemp + "<div style=\"margin-bottom:20px;\">\r\n";
            iTemp = iTemp +
                    "<a onclick=\"selectProce(2)\" target=\"_self\"><img src = \"http://www.skymobile.com.cn/shop/data/brandlogo/1284637613303364367.jpg\" height = \"45\" width = \"90\" style = \"cursor:pointer\"></a>\r\n";
            //APPLE
            iTemp = iTemp +
                    "<a onclick=\"selectProce(3)\" target=\"_self\"><img src = \"http://www.skymobile.com.cn/shop/data/brandlogo/1284637622507471403.jpg\" height = \"45\" width = \"90\" style = \"cursor:pointer\"></a>\r\n";
            //HTC
            iTemp = iTemp +
                    "<a onclick=\"selectProce(4)\" target=\"_self\"><img src = \"http://www.skymobile.com.cn/shop/data/brandlogo/1284637628666595023.jpg\" height = \"45\" width = \"90\" style = \"cursor:pointer\"></a>\r\n";
            //SAMSUNG
            iTemp = iTemp +
                    "<a onclick=\"selectProce(6)\" target=\"_self\"><img src = \"http://www.skymobile.com.cn/shop/data/brandlogo/1284637645717487167.jpg\" height = \"45\" width = \"90\" style = \"cursor:pointer\"></a>\r\n";
            //moto
            iTemp = iTemp +
                    "<a onclick=\"selectProce(5)\" target=\"_self\"><img src = \"http://www.skymobile.com.cn/shop/data/brandlogo/1284637637312112693.jpg\" height = \"45\" width = \"90\" style = \"cursor:pointer\"></a>\r\n";
            //sony
            iTemp = iTemp +
                    "<a onclick=\"selectProce(10)\" target=\"_self\"><img src = \"http://www.skymobile.com.cn/shop/data/brandlogo/1293583024943383929.jpg\" height = \"45\" width = \"90\" style = \"cursor:pointer\"></a>\r\n";
            //dell
            iTemp = iTemp +
                    "<a onclick=\"selectProce(1)\" target=\"_self\"><img src = \"http://www.skymobile.com.cn/shop/data/brandlogo/1284637601255997769.jpg\" height = \"45\" width = \"90\" style = \"cursor:pointer\"></a>\r\n";
            //nokia
            iTemp = iTemp +
                    "<a onclick=\"selectProce(7)\" target=\"_self\"><img src = \"http://www.skymobile.com.cn/shop/data/brandlogo/1284637653897428284.jpg\" height = \"45\" width = \"90\" style = \"cursor:pointer\"></a>\r\n";
            //lg
            iTemp = iTemp +
                    "<a onclick=\"selectProce(8)\" target=\"_self\"><img src = \"http://www.skymobile.com.cn/shop/data/brandlogo/1284637660688113389.jpg\" height = \"45\" width = \"90\" style = \"cursor:pointer\"></a>\r\n";
            //sharp
            iTemp = iTemp + "</div>\r\n";
            iTemp = iTemp + "<div id=\"list\">\r\n";
            iTemp = iTemp + "<script language=javascript>\r\n";
            iTemp = iTemp + "function selectProce(n)\r\n";
            iTemp = iTemp + "{\r\n";
            iTemp = iTemp + "var box = $(\"box\");\r\n";
            iTemp = iTemp + "box = document.getElementById(\"PriceList\").getElementsByTagName(\"div\")\r\n";
            iTemp = iTemp + "for (i=0;i<box.length;i++)\r\n";
            iTemp = iTemp + "{\r\n";
            iTemp = iTemp + "box[i].style.display=\"none\";\r\n";
            iTemp = iTemp + "}\r\n";
            iTemp = iTemp + "document.getElementById(\"Price\"+n).style.display=\"\";\r\n";
            iTemp = iTemp + "}\r\n";
            iTemp = iTemp + "</script>\r\n";

            iTemp = iTemp + "<div id=\"PriceList\">\r\n";

            #region 运算获得页面

            //苹果
            iTemp = iTemp + GetSingleHTML(iPrice, "2");
            //HTC
            iTemp = iTemp + GetSingleHTML(iPrice, "3");
            //SAMSUNG
            iTemp = iTemp + GetSingleHTML(iPrice, "4");
            //MOTO
            iTemp = iTemp + GetSingleHTML(iPrice, "6");
            //SONY
            iTemp = iTemp + GetSingleHTML(iPrice, "5");
            //DELL
            iTemp = iTemp + GetSingleHTML(iPrice, "10");
            //NOKIA
            iTemp = iTemp + GetSingleHTML(iPrice, "1");
            //LG
            iTemp = iTemp + GetSingleHTML(iPrice, "7");
            //SHARP
            iTemp = iTemp + GetSingleHTML(iPrice, "8");

            #endregion



            #region 1.2版本页面 页脚

            iTemp = iTemp + "</div>\r\n";
            iTemp = iTemp + "</div>\r\n";

            #region GOOGLE AD

            iTemp = iTemp + "<div align = center>\r\n";
            iTemp = iTemp + "<script type=\"text/javascript\"><!--\r\n";
            iTemp = iTemp + "google_ad_client = \"ca-pub-5853993113305574\";\r\n";
            iTemp = iTemp + "/* 龙翔通讯 */\r\n";
            iTemp = iTemp + "google_ad_slot = \"9099981090\";\r\n";
            iTemp = iTemp + "google_ad_width = 728;\r\n";
            iTemp = iTemp + "google_ad_height = 90;\r\n";
            iTemp = iTemp + "//-->\r\n";
            iTemp = iTemp + "</script>\r\n";
            iTemp = iTemp + "<script type=\"text/javascript\"\r\n";
            iTemp = iTemp + "src=\"http://pagead2.googlesyndication.com/pagead/show_ads.js\">\r\n";
            iTemp = iTemp + "</script>\r\n";
            iTemp = iTemp + "</div>\r\n";

            #endregion

            //iTemp = iTemp + "</div>";
            iTemp = iTemp + "<div id=\"footer\">\r\n";
            iTemp = iTemp + "<div id=\"footer_top\">\r\n";
            iTemp = iTemp + "<p class=\"good_link\">\r\n";
            iTemp = iTemp + "<a href=\"http://www.skymobile.com.cn/index.php\">龙翔通讯</a> | \r\n";
            iTemp = iTemp + "<a href=\"http://www.skymobile.com.cn/index.php/action-site-type-map\">站点地图</a>\r\n";
            iTemp = iTemp + "</p>\r\n";
            iTemp = iTemp + "</div>\r\n";
            iTemp = iTemp + "<div class=\"copyright\">\r\n";
            iTemp = iTemp + "<p id=\"copyright\">\r\n";
            iTemp = iTemp + "Powered by 龙翔通讯 &copy; 2004-2011 \r\n";
            iTemp = iTemp + "</p>\r\n";
            iTemp = iTemp + "<script src=\"http://s17.cnzz.com/stat.php?id=3577145&web_id=3577145&show=pic1\" language=\"JavaScript\"></script>";
            iTemp = iTemp + "</div>\r\n";
            iTemp = iTemp + "</div>\r\n";
            iTemp = iTemp + "</body>\r\n";
            iTemp = iTemp + "</html>\r\n";

            #endregion

            #endregion

            try
            {
                File.WriteAllText(Path + @"\DailyPrice.htm", iTemp);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string GetSingleHTML(LXPrice[] iPrice, string BrandID)
        {
            string iTemp = "";
            if (BrandID == "2")
            {
                iTemp = "<div id=Price" + BrandID + "  class=\"flp\">\r\n";
            }
            else
            {
                iTemp = "<div id=Price" + BrandID + "  class=\"flp\" style=\"display:none\">\r\n";
            }
            iTemp = iTemp + "";
            //iTemp = iTemp + "<hr>\r\n";


            for (int i = 0; i < iPrice.Length; i++)
            {
                if (iPrice[i].iBrand == BrandID)
                {
                    iTemp = iTemp + "<hr>\r\n";
                    iTemp = iTemp + "<dl>\r\n";
                    iTemp = iTemp + "<dd style=\"height:90; width:90\"><center><IMG height=90 src=\"" + iPrice[i].iImg +
                            "\"></center></dd>\r\n";
                    iTemp = iTemp + "<dt style=\"margin-bottom:5px\">\r\n";
                    iTemp = iTemp + "<a href=\"" + iPrice[i].iURL + "\" target=\"_new\">" + iPrice[i].iName + "</a>\r\n";
                    iTemp = iTemp + "<p>\r\n";
                    iTemp = iTemp + "<font color = \"Firebrick\">概要说明 :</font>" + iPrice[i].iFullName + "\r\n";
                    iTemp = iTemp + "</p>\r\n";
                    iTemp = iTemp + "<p>\r\n";
                    iTemp = iTemp + "<font color = \"maroon\">龙翔配置 :</font>" + iPrice[i].iEquip + "\r\n";
                    iTemp = iTemp + "</p>\r\n";
                    iTemp = iTemp + "<p>\r\n";
                    iTemp = iTemp + "<font color = \"seagreen\">龙翔点评 :</font>" + iPrice[i].iDES + "\r\n";
                    iTemp = iTemp + "</p>\r\n";
                    iTemp = iTemp + "</dt>\r\n";
                    iTemp = iTemp + "</dl>\r\n";
                    iTemp = iTemp + "<span style=\"float:right\">\r\n";
                    for (int j = 0; j < iPrice[i].iCP.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(iPrice[i].iCP[j].iColor))
                        {
                            //求颜色+价格
                            iTemp = iTemp + "<font color=\"#000066\" size=\"1\">" + iPrice[i].iCP[j].iColor +
                                    "</font>:<font color=\"#FF0000\" size=\"1\"><b>" + iPrice[i].iCP[j].iPrice +
                                    "</b></font><br>\r\n";
                        }
                    }
                    iTemp = iTemp + "</span>\r\n";
                }
            }
            iTemp = iTemp + "</div>\r\n";
            return iTemp;
        }

        private static ColorPrice[] GetCP(string itemID)
        {
            var iReturn = new ColorPrice[50];

            string mySelectQuery =
                "SELECT attr_value ,attr_price FROM ecs_goods_attr WHERE attr_id = 82 AND goods_id = " + itemID +
                " ORDER BY attr_price;";
            var iConnection =
                new MySqlConnection(
                    "Server=127.0.0.1;Uid=root;Pwd=;Database=skymobile;Encrypt=true;Compress=true;");

            var myCommand = new MySqlCommand(mySelectQuery, iConnection);

            iConnection.Open();
            MySqlDataReader myReader;
            myReader = myCommand.ExecuteReader();
            int i = 0;
            while (myReader.Read())
            {
                iReturn[i].iColor = myReader.GetString(0);
                iReturn[i].iPrice = myReader.GetString(1);
                i++;
            }
            myReader.Close();
            iConnection.Close();
            iConnection.Dispose();
            return iReturn;
        }

        #region Nested type: ColorPrice

        public struct ColorPrice
        {
            public string iColor;
            public string iPrice;
        }

        #endregion

        #region Nested type: LXPrice

        public struct LXPrice
        {
            public string iBrand;
            public ColorPrice[] iCP;
            public string iFullName;
            public string iDES;
            public string iEquip;
            public string iID;
            public string iImg;
            public string iName;
            public string iURL;
        }

        #endregion
    }
}
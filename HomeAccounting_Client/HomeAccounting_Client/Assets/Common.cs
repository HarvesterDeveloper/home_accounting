using System;
using System.Collections.Generic;
using System.Linq;
using HomeAccounting_Client.Assets.Database;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Text.RegularExpressions;

namespace HomeAccounting_Client.Assets
{
    public static class Model
    {
        #region Variables

        private static String _CaptchaToUser = "";
        private static Boolean _CaptchaSolved = false;
        private static Int32 _UserId = -1;
        private static home_accountingEntities _Database = null;

        #endregion

        #region Functions

        /// <summary>
        /// This function trying to make connection with MSSQL server. Returns string that contain error messages. If function return empty string, its mean that all worked fine.
        /// </summary>
        /// <returns></returns>
        public static String ConnectToDatabase()
        {
            try { _Database = new home_accountingEntities(); }
            catch (Exception IncomeException) { return IncomeException.Message; }
            if (_Database == null) return "Ошибка подключения к бд"; else return "";
        }

        /// <summary>
        /// This function trying to authorize user from incomed data. Returns string that contain error messages. If function return empty string, its mean that all worked fine.
        /// </summary>
        /// <param name="incomingLogin"></param>
        /// <param name="incomingPassword"></param>
        /// <returns></returns>
        public static String Authorize(String incomingLogin, String incomingPassword)
        {
            member user = null;

            try { user = _Database.members.SingleOrDefault(U => U.name == incomingLogin && U.password == incomingPassword);}
            catch(Exception IncomeException) { return IncomeException.Message;}
            if (user == null) return "Указаны некорректные данные";
            else
            {
                _UserId = user.id;
                return "";
            }
        }

        /// <summary>
        /// These function requests and returns new captcha, also clear solved capthca state.
        /// </summary>
        /// <returns></returns>
        public static String RequestCaptcha()
        {
            _CaptchaSolved = false;
            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";

            char[] a = { ',' };

            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = " ";
            Random r = new Random();
            int constat = r.Next(6, 12);
            for (int i = 0; i < constat; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }
            _CaptchaToUser = pwd;
            return pwd;
        }

        /// <summary>
        /// This function checks captcha input correctnes. Its returns empty string if everything works fine else returns error message.
        /// </summary>
        /// <param name="incomeCaptcha"></param>
        /// <returns></returns>
        public static String CheckCaptcha(String incomeCaptcha)
        {
            if(_CaptchaToUser!="")
            {
                if (incomeCaptcha == _CaptchaToUser)
                {
                    _CaptchaSolved = true;
                    return "";
                }
                else return "Каптча введена неверно.";
            }
            else
            {
                return "Нельзя проверить каптчу не запросив её.";
            }
        }

        /// <summary>
        /// This function register a new user. If everything workes fine it returns empty string, else returns error.
        /// </summary>
        /// <param name="incomingLogin"></param>
        /// <param name="incomingPassword"></param>
        /// <returns></returns>
        public static String RequestRegistration(String incomingLogin, String incomingPassword)
        {
            if(_CaptchaSolved==true)
            {
                string smallWords = @"[a-z]";
                string bigWords = @"[A-Z]";
                string numbers = @"[0-9]";
                string symbolSpec = @"[@,`,$,^,%,*,&,\s]";

                if (incomingPassword.Length >= 8)
                {
                    if (Regex.IsMatch(incomingPassword, smallWords))
                    {
                        if (Regex.IsMatch(incomingPassword, bigWords))
                        {
                            if (Regex.IsMatch(incomingPassword, numbers))
                            {
                                if (Regex.IsMatch(incomingPassword, symbolSpec))
                                {
                                    member newUser = new member();
                                    newUser.name = incomingLogin;
                                    newUser.password = incomingPassword;
                                    try
                                    {
                                        _Database.members.Add(newUser);
                                        _Database.SaveChanges();
                                        _CaptchaSolved = false;
                                        return "";
                                    }
                                    catch(Exception ex)
                                    {
                                        return ex.Message;
                                    }
                                }
                                else
                                {
                                    return "Добавьте спецсимволы!";
                                }
                            }
                            else
                            {
                                return "Добавьте цифры!";
                            }
                        }
                        else
                        {
                            return "Добавьте заглавные буквы!";
                        }
                    }
                    else
                    {
                        return "Добавьте строчные буквы!";
                    }
                }
                else
                {
                    return "Символов должнобыть больше 8!";
                }
            }
            else
            {
                return "Регистрация невозможна до ввода капчи.";
            }
        }

        /// <summary>
        /// This function returns List of logged user. If exception throws it returns null.
        /// </summary>
        /// <returns></returns>
        public static List<action> GetUserActionsList()
        {
            List<action> functionResult = null;

            try
            {
                functionResult = _Database.actions.Where(action => action.owner_id == _UserId).ToList();
            }
            catch(Exception exception)
            {
                //
            }

            return functionResult;
        }

        /// <summary>
        /// This function returns list of all reports in strings for current user.
        /// </summary>
        /// <returns></returns>
        public static List<String> GetUserReports()
        {
            List<String> functionResult = new List<String>();
            List<action> userActions = _Database.actions.Where(action => action.owner_id == _UserId).ToList();
            List<DateTime> availableMonths = new List<DateTime>();

            // searching available months
            foreach (action act in userActions) if (availableMonths.Contains(new DateTime(act.date.Value.Year, act.date.Value.Month, 1)) == false) availableMonths.Add(new DateTime(act.date.Value.Year, act.date.Value.Month, 1));

            // generating reports
            availableMonths.Sort();
            if (availableMonths.Count > 0)
            {
                foreach(DateTime curMonth in availableMonths)
                {
                    // searching month related actions
                    List<action> monthRelatedActions = new List<action>();

                    foreach (action act in userActions) if (act.date.Value.Month == curMonth.Month && act.date.Value.Year == curMonth.Year) monthRelatedActions.Add(act);

                    // analyze actions and calc stats
                    Decimal monthIncome = 0;
                    Decimal monthOutcome = 0;
                    action biggestSpentAction = new action(); biggestSpentAction.amount = 0;
                    action biggestIncomeAction = new action(); biggestIncomeAction.amount = 0;
                    Decimal goalsIncome = 0;
                    Decimal[] wastedOnGoals = new Decimal[4096];

                    foreach(action act in monthRelatedActions)
                    {
                        if (act.is_income == true) monthIncome += (Decimal)act.amount; else monthOutcome += (Decimal)act.amount;
                        if (act.is_income == false && act.amount > biggestSpentAction.amount) biggestSpentAction = act;
                        if (act.is_income == true && act.amount > biggestIncomeAction.amount) biggestIncomeAction = act;
                        if (act.goal_id > -1 && act.is_income==true) goalsIncome += (Decimal)act.amount;
                    }

                    foreach(action act in userActions)
                    {
                        if(act.goal_id > -1)
                        {
                            wastedOnGoals[(int)act.goal_id] += (Decimal)act.amount;
                        }
                    }

                    // generate new report by getted stats
                    String newReport = "";

                    newReport += curMonth.ToString("Y") + Environment.NewLine;
                    newReport += "------------------------------------------" + Environment.NewLine;
                    newReport += "Статистика по общему анализу" + Environment.NewLine;
                    newReport += "------------------------------------------" + Environment.NewLine;
                    newReport += "Ваш доход в этом месяце составил: " + monthIncome + "." + Environment.NewLine;
                    newReport += "Ваши расходы в этом месяце составили: " + monthOutcome + "." + Environment.NewLine;
                    newReport += "Результат:" + ((int)(monthIncome - monthOutcome)) + "." + Environment.NewLine;
                    newReport += "Результат в соотношении:" +  ((double)monthIncome/((double)(monthIncome + monthOutcome)/100.00)) + "% / " + ((double)monthOutcome / ((double)(monthIncome + monthOutcome) / 100.00)) + "%"+Environment.NewLine;
                    newReport += "Наибольшей тратой было " + biggestSpentAction.source + "( " + biggestSpentAction.commentary + " )"+Environment.NewLine;
                    newReport += "Наибольшей прибылью было " + biggestIncomeAction.source + "( " + biggestIncomeAction.commentary + " )"+Environment.NewLine;
                    newReport += "------------------------------------------" + Environment.NewLine;
                    newReport += "Статистика по целям" + Environment.NewLine;
                    newReport += "------------------------------------------" + Environment.NewLine;
                    newReport += "Добавлено к целям: " + goalsIncome.ToString() + Environment.NewLine;
                    foreach (goal gl in GetUserGoalsList())
                    {
                        newReport += "Цель " + gl.name + "( " + gl.description + " ) выполнена на " + wastedOnGoals[gl.id].ToString() + "/" + gl.goal_money.ToString() + " ("+(wastedOnGoals[gl.id]/(gl.goal_money / 100)).ToString()+"%/100%)" + Environment.NewLine;
                    }

                    // add report to result
                    functionResult.Add(newReport);
                }
            }
            else
            {
                functionResult.Add("Нехватка данных");
            }

            return functionResult;
        }

        /// <summary>
        /// This function returns goal class objects list of all user goals. If search failed it will return empty list.
        /// </summary>
        /// <returns></returns>
        public static List<goal> GetUserGoalsList()
        {
            List<goal> functionResult = new List<goal>();

            try
            {
                return _Database.goals.Where(goal => goal.owner_id == _UserId).ToList();
            }
            catch(Exception exception)
            {
                return functionResult;
            }
        }

        /// <summary>
        /// This function returns strings list of all user goals formated by type "goal id|goal name", for example "2|For server". If search failed it will return empty list.
        /// </summary>
        /// <returns></returns>
        public static List<String> GetUserGoalStringed()
        {
            List<String> functionResult = new List<String>();
            List<goal> userGoals = GetUserGoalsList();
            foreach (goal gl in userGoals) functionResult.Add(gl.id+"|"+gl.name);

            return functionResult;
        }

        /// <summary>
        /// This function returns goals with given Id. If excetpion throws it returns null.
        /// </summary>
        /// <returns></returns>
        public static goal GetGoalById(Int32 incomingGoalId)
        {
            goal functionResult = null;

            try
            {
                functionResult = _Database.goals.SingleOrDefault(relgoal => relgoal.id == incomingGoalId);
            }
            catch (Exception exception)
            {
                //
            }

            return functionResult;
        }

        /// <summary>
        /// This function adds new action in database with given args.
        /// </summary>
        /// <param name="incomingGoalId"></param>
        /// <param name="incomingSource"></param>
        /// <param name="incomingIsIncome"></param>
        /// <param name="incomingAmount"></param>
        /// <param name="incomingDate"></param>
        /// <param name="incomingCommentary"></param>
        /// <returns></returns>
        public static String AddAction(Int32 incomingGoalId, String incomingSource, Boolean incomingIsIncome, Decimal incomingAmount, DateTime incomingDate, String incomingCommentary)
        {
            action newAction = new action();
            newAction.owner_id = _UserId;
            newAction.goal_id = incomingGoalId;
            newAction.source = incomingSource;
            newAction.is_income = incomingIsIncome;
            newAction.amount = incomingAmount;
            newAction.date = incomingDate;
            newAction.commentary = incomingCommentary;

            try
            {
                _Database.actions.Add(newAction);
                _Database.SaveChanges();
            }
            catch (Exception ex) { return ex.Message; }

            return "";
        }

        /// <summary>
        /// This function updates action with given args. If errors meeted it will return error message.
        /// </summary>
        /// <param name="incomingId"></param>
        /// <param name="incomingGoalId"></param>
        /// <param name="incomingSource"></param>
        /// <param name="incomingIsIncome"></param>
        /// <param name="incomingAmount"></param>
        /// <param name="incomingDate"></param>
        /// <param name="incomingCommentary"></param>
        /// <returns></returns>
        public static String UpdateAction(Int32 incomingId, Int32 incomingGoalId, String incomingSource, Boolean incomingIsIncome, Decimal incomingAmount, DateTime incomingDate, String incomingCommentary)
        {
            action newAction = _Database.actions.SingleOrDefault(act => act.id == incomingId);
            newAction.owner_id = _UserId;
            newAction.goal_id = incomingGoalId;
            newAction.source = incomingSource;
            newAction.is_income = incomingIsIncome;
            newAction.amount = incomingAmount;
            newAction.date = incomingDate;
            newAction.commentary = incomingCommentary;

            try
            {
                _Database.SaveChanges();
            }
            catch (Exception ex) { return ex.Message; }

            return "";
        }

        /// <summary>
        /// This function returns empty string if goals add ok, else its return error message.
        /// </summary>
        /// <param name="incomingName"></param>
        /// <param name="incomingDescription"></param>
        /// <param name="incomingGoalMoney"></param>
        /// <returns></returns>
        public static String AddGoal(String incomingName, String incomingDescription, Decimal incomingGoalMoney)
        {
            goal newGoal = new goal();
            newGoal.owner_id = _UserId;
            newGoal.name = incomingName;
            newGoal.description = incomingDescription;
            newGoal.goal_money = incomingGoalMoney;

            try
            {
                _Database.goals.Add(newGoal);
                _Database.SaveChanges();
                return "";
            }
            catch(Exception exception)
            {
                return exception.Message;
            }

            
        }

        /// <summary>
        /// This function deletes action from db. If exception throws its return exception message, if succes then return empty string;
        /// </summary>
        /// <param name="incomingAction"></param>
        /// <returns></returns>
        public static String DeleteAction(action incomingAction)
        {
            String functionResult = "";

            try
            {
                _Database.actions.Remove(incomingAction);
                _Database.SaveChanges();
            }
            catch(Exception exception)
            {
                functionResult = exception.Message;
            }

            return functionResult;
        }

        /// <summary>
        /// This function puts given action in db. If exception throws its return exception message, if succes then return empty string;
        /// </summary>
        /// <param name="incomingAction"></param>
        /// <returns></returns>
        public static String PutAction(action incomingAction)
        {
            String functionResult = "";

            try
            {
                _Database.actions.Add(incomingAction);
                _Database.SaveChanges();
            }
            catch (Exception exception)
            {
                functionResult = exception.Message;
            }

            return functionResult;
        }

        /// <summary>
        /// This function deletes goal from db. If exception throws its return exception message, if succes then return empty string;
        /// </summary>
        /// <param name="incomingGoal"></param>
        /// <returns></returns>
        public static String DeleteGoal(goal incomingGoal)
        {
            String functionResult = "";

            try
            {
                _Database.goals.Remove(incomingGoal);
                _Database.SaveChanges();
            }
            catch (Exception exception)
            {
                functionResult = exception.Message;
            }

            return functionResult;
        }

        #endregion
    }

    #region Converters

    public class IncomeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.ToString() == "True") return "Доход"; else return "Расход";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToBoolean(value);
        }
    }

    public class DatetimeConverter : IValueConverter
    {
        public object Convert(object value, Type typeType, object parametr, CultureInfo culture)
        {
            if (value != null) return ((DateTime)value).ToString("dd.MM.yyyy HH:mm:ss"); else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class GoalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Int32 incomingId = Int32.Parse(value.ToString());

            if(incomingId > -1)
            {
                goal relatedGoal = Model.GetGoalById(incomingId);
                if (relatedGoal != null) return relatedGoal.name; else return "Нет";

            }
            else
            {
                return "Нет";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    #endregion
}
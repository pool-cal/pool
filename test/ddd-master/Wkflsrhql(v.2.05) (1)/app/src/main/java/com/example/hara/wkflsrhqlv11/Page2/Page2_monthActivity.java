package com.example.hara.wkflsrhqlv11.Page2;

import android.graphics.Color;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.example.hara.wkflsrhqlv11.CreateID_UUID;
import com.example.hara.wkflsrhqlv11.DBConnect.Goal;
import com.example.hara.wkflsrhqlv11.DBConnect.Value;
import com.example.hara.wkflsrhqlv11.R;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.util.ArrayList;

public class Page2_monthActivity extends Fragment {
    public TextView textview;
    public View view;
    public CreateID_UUID createID_uuid;
    String PhoneNum;
    ArrayList<String> myDataset;
    ArrayList<String> myGoalDataset;

    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {


        view=inflater.inflate(R.layout.page2_month, container, false);
        // TextView textView=(TextView)view.findViewById(R.id.month_goal);
        createID_uuid=new CreateID_UUID();
        PhoneNum=(String) createID_uuid.getUniqueID(view.getContext());
        Task task=new Task();
        task.execute("List_day",PhoneNum);
        task.execute("Get_list_goal",PhoneNum);
        //리스트
         return view;


    }

    class Task extends AsyncTask<String, Void, String> {
        Goal goal;

        protected void onPostExecute(String result) {
            if (result.charAt(0) == '1') {
                //리스트 day 항목
                // ArrayList<String> list_week = new ArrayList<String>();
                int length = 4;
                int count = 1;
//                goal = new Goal();
//                String[] result_split = result.split("<1>");
//                String[] result_list;
//
//                while (count < result_split.length - 1) {
//                    result_list = result_split[count].split("<br/>");
//                    Log.i("GET_WEEK_LIST", "GET_WEEK_LIST : " + result_list[0]);
//                    list_week.add(result_list[0]);
//                count++;
                    /*
                    리스트 설정 파라미터는  result_list[0]
                    */

                goal = new Goal();
                String[] result_split = result.split("<br/>");
                String[] result_list;

                for(int i=1; i<result_split.length; i++){
                    result_list = result_split[i].split("<1>");
                    myDataset.add(result_list[1]);
                }

                // myDataset index(1-31)
                //Goal.setList_WeekSum(list_week);
                //int sum=0;
                //for(int go=0; go<list_week.size(); go++)
                  //  sum+= Integer.parseInt(list_week.get(go));

                //TextView textView= (TextView)view.findViewById(R.id.month_goal);
               //textView.setText(sum+"원 입니다");

            }
            else if (result.charAt(0) == '2') {
                goal = new Goal();
                String[] result_split = result.split("<br/>");
                String[] result_list;

                for(int i=1; i<result_split.length; i++){
                    result_list = result_split[i].split("<1>");
                    myGoalDataset.add(result_list[1]);
                }
                String pre_array = "R.id.page2_day_";

                for(int number =1; number < myDataset.size(); number++){
                    pre_array+=""+(number);
                    TextView textView= (TextView)view.findViewById(Integer.parseInt(pre_array));
                    if(Integer.parseInt(myDataset.get(number)) < Integer.parseInt(myGoalDataset.get(number))){
                        textView.setBackgroundColor(Color.argb(0,1,2,3));
                    }
                    else{
                        textView.setBackgroundColor(Color.argb(0,1,2,3));
                    }
                }

            }
        }

            @Override
            protected String doInBackground (String...params){
                String method = params[0];
                String errorString = null;
                String User_Number=params[1];
                if (method.equals("List_day")) {
                    try {
                        URL url = new URL(Value.List_day);
                        HttpURLConnection httpURLConnection = (HttpURLConnection) url.openConnection();
                        httpURLConnection.setRequestMethod("POST");
                        httpURLConnection.setDoOutput(true);
                        OutputStream OS = httpURLConnection.getOutputStream();
                        BufferedWriter bufferedWriter = new BufferedWriter(new OutputStreamWriter(OS, "UTF-8"));
                        String data = /*php*/URLEncoder.encode("User_Number", "UTF-8") + "=" + URLEncoder.encode(User_Number, "UTF-8");

                        bufferedWriter.write(data);
                        bufferedWriter.flush();
                        bufferedWriter.close();
                        OS.close();

                        httpURLConnection.setReadTimeout(5000);
                        httpURLConnection.setConnectTimeout(5000);
                        httpURLConnection.connect();

                        int responseStatusCode = httpURLConnection.getResponseCode();
                        Log.d("List_day", "response code - " + responseStatusCode);

                        InputStream inputStream;
                        if (responseStatusCode == HttpURLConnection.HTTP_OK) {
                            inputStream = httpURLConnection.getInputStream();
                        } else {
                            inputStream = httpURLConnection.getErrorStream();
                        }
                        InputStreamReader inputStreamReader = new InputStreamReader(inputStream, "UTF-8");
                        BufferedReader bufferedReader = new BufferedReader(inputStreamReader);

                        StringBuilder sb = new StringBuilder();
                        String line;

                        while ((line = bufferedReader.readLine()) != null) {
                            sb.append(line);
                        }
                        bufferedReader.close();

                        return 1+ "<br/>" + sb.toString().trim();

                    } catch (Exception e) {
                        Log.d("List_day", "InsertData: Error ", e);
                        errorString = e.toString();
                        return null;
                    }
                }

                else if (method.equals("Get_list_goal")) {
                    try {
                        URL url = new URL(Value.Get_list_goal);
                        HttpURLConnection httpURLConnection = (HttpURLConnection) url.openConnection();
                        httpURLConnection.setRequestMethod("POST");
                        httpURLConnection.setDoOutput(true);
                        OutputStream OS = httpURLConnection.getOutputStream();
                        BufferedWriter bufferedWriter = new BufferedWriter(new OutputStreamWriter(OS, "UTF-8"));
                        String data = /*php*/URLEncoder.encode("User_Number", "UTF-8") + "=" + URLEncoder.encode(User_Number, "UTF-8");

                        bufferedWriter.write(data);
                        bufferedWriter.flush();
                        bufferedWriter.close();
                        OS.close();

                        httpURLConnection.setReadTimeout(5000);
                        httpURLConnection.setConnectTimeout(5000);
                        httpURLConnection.connect();

                        int responseStatusCode = httpURLConnection.getResponseCode();
                        Log.d("SELECT_List_GET", "response code - " + responseStatusCode);

                        InputStream inputStream;
                        if (responseStatusCode == HttpURLConnection.HTTP_OK) {
                            inputStream = httpURLConnection.getInputStream();
                        } else {
                            inputStream = httpURLConnection.getErrorStream();
                        }

                        InputStreamReader inputStreamReader = new InputStreamReader(inputStream, "UTF-8");
                        BufferedReader bufferedReader = new BufferedReader(inputStreamReader);

                        StringBuilder sb = new StringBuilder();
                        String line;

                        while ((line = bufferedReader.readLine()) != null) {
                            sb.append(line);
                        }
                        bufferedReader.close();

                        return 2+ "<br/>" + sb.toString().trim();

                    } catch (Exception e) {
                        Log.d("Get_list_goal", "InsertData: Error ", e);
                        errorString = e.toString();
                        return null;
                    }
                }
                else
                    return null;
            }
        }
}
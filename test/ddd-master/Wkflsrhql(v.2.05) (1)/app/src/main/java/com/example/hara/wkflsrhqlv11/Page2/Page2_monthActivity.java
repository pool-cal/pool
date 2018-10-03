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
    ArrayList<String> myDatasets;
    ArrayList<String> myGoalDataset;

    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {


        view=inflater.inflate(R.layout.page2_month, container, false);
        // TextView textView=(TextView)view.findViewById(R.id.month_goal);
        createID_uuid=new CreateID_UUID();
        PhoneNum=(String) createID_uuid.getUniqueID(view.getContext());
        Task task=new Task();
        Task task2 = new Task();
        task.execute("Goal_get_daysum",PhoneNum);
        task2.execute("Get_list_goal",PhoneNum);
        //리스트
         return view;


    }

    class Task extends AsyncTask<String, Void, String> {
        protected void onPostExecute(String result) {
            if (result.charAt(0) == '1') {
                String[] result_split = result.split("<br/>");
                String[] result_list;
                myDatasets = new ArrayList<String>();
                for(int i=1; i<result_split.length; i++){
                    result_list = result_split[i].split("<1>");
                    myDatasets.add(result_list[1]);
                }
            }
            else if (result.charAt(0) == '2') {
                Log.i("Get_list_goal", "Get_list_goal_page2 : " +result);
                String[] result_split2 = result.split("<br/>");
                String[] result_list2;
                myGoalDataset = new ArrayList<String>();
                for(int i=1; i<result_split2.length; i++){
                    result_list2 = result_split2[i].split("<1>");
                    myGoalDataset.add(result_list2[1]);
                }
                int resID[] = {R.id.page2_day_1,R.id.page2_day_2,R.id.page2_day_3,R.id.page2_day_4,R.id.page2_day_5, R.id.page2_day_6, R.id.page2_day_7,
                        R.id.page2_day_8, R.id.page2_day_9,R.id.page2_day_10,R.id.page2_day_11,R.id.page2_day_12,R.id.page2_day_13,R.id.page2_day_14,R.id.page2_day_15,
                        R.id.page2_day_16,R.id.page2_day_17,R.id.page2_day_18,R.id.page2_day_19,R.id.page2_day_20,R.id.page2_day_21, R.id.page2_day_22, R.id.page2_day_23,
                        R.id.page2_day_24,R.id.page2_day_25,R.id.page2_day_26,R.id.page2_day_27,R.id.page2_day_28,R.id.page2_day_29,R.id.page2_day_30,R.id.page2_day_31};
                int count=0;
                // 색상 목표치 이하일 때 초록색, 그 이상일 때 빨간색
                for(int number =0; number < myDatasets.size(); number++){
                    TextView textView= (TextView)view.findViewById(resID[number]);
                    if(Integer.parseInt(myDatasets.get(number)) > Integer.parseInt(myGoalDataset.get(number))){
                        Log.i("page2_myDatasets", "Integer.parseInt(myDatasets.get(number)) : " +Integer.parseInt(myDatasets.get(number)));
                        Log.i("page2_myGoalDataset", "Integer.parseInt(myGoalDataset.get(number)) : " +Integer.parseInt(myGoalDataset.get(number)));
                        textView.setBackgroundColor(Color.RED);
                        count++;
                    }
                    else{
                        Log.i("page2_myDatasets_else", "Integer.parseInt(myDatasets.get(number)) : " +Integer.parseInt(myDatasets.get(number)));
                        Log.i("page2_myGoalDatast_else", "Integer.parseInt(myGoalDataset.get(number)) : " +Integer.parseInt(myGoalDataset.get(number)));
//                        Log.i("page2_myGoalDatast_else", "Integer.parseInt(myGoalDataset.get(number)) : " +number);
                        textView.setBackgroundColor(Color.GREEN);
                    }
                }
                //  1등급 ~ 6등급
                // count <=5 1등급
                // count <=10 2등급
                // count <= 15 3등급
                // count <= 20 4등급
                // count <= 25 5등급
                // count <= 31 6등급
                Log.i("count_page2", "count " +count);
                TextView textView2 = (TextView)view.findViewById(R.id.age);
                if(count<=5)
                    textView2.setText("1등급");
                else if(count>5 && count <=10)
                        textView2.setText("2등급");
                else if(count>10 && count <=15)
                        textView2.setText("3등급");
                else if(count>15 && count <=20)
                        textView2.setText("4등급");
                else if(count>20 && count <=25)
                        textView2.setText("5등급");
                else if(count>25 && count <=31)
                        textView2.setText("6등급");
                else
                        textView2.setText("6등급");
            }
        }
            @Override
            protected String doInBackground (String...params){
                String method = params[0];
                String errorString = null;
                String User_Number=params[1];
                if (method.equals("Goal_get_daysum")) {
                    try {
                        URL url = new URL(Value.Goal_get_daysum);
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
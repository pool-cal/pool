package com.example.hara.wkflsrhqlv11.MMS;

import java.util.HashMap;


public class CardValue {
    private static HashMap<String , String> hashMap=new HashMap();
    CardValue(){
        hashMap.put("15447200","신한");
    }
    public static void setHashMap(HashMap<String, String> hashMap) {
        CardValue.hashMap = hashMap;
    }

    public static HashMap<String, String> getHashMap() {
        return hashMap;
    }
}

# SamsApi
通用API_demo api

demo 系統

Api需求項目：
1.	放趟（SetDailyServiceTrips）
提供放趟Api，讓外部車隊系統，將放趟資料直接寫入系統Db

2.	取得預約任務（GetDailyBookings）
提供取得預約任務Api，讓外部車隊系統，取得系統從訂車客服接到的預約任務

3.	匯入班表（ImportDailyDriverSchedule）
提供匯入班表Api，讓外部車隊，將以編排好的司機班表，直接匯入系統

4.	查詢趟次狀態（QueryTripStatus）
提供查詢趟次Api讓外部車隊，查詢每筆任務的執行進度，前往狀態後，任務狀態在從Booking轉到CallTaxi資料表異動。






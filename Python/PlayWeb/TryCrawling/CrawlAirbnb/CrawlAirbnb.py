
import requests
from bs4 import BeautifulSoup
import json

room_url = "https://www.airbnb.com/rooms/10705179"
request = requests.get(room_url)
soup = BeautifulSoup(request.content)

title = soup.find('h1',{"id":"listing_name"})

reviews = soup.findAll('div',{'class':'row review'})
reviews

url = "https://api.airbnb.com/v2/reviews?client_id=3092nxybyb0otqw18e8nh5nty&listing_id=9460&role=all&_limit=2&_offset=2"
request = requests.get(url)
request.content
d = json.loads(request.content)

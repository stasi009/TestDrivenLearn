
import json
import requests
from bs4 import BeautifulSoup

class Restaurant(object):
    def __init__(self,name):
        self.name = name
        self.address = ""
        self.telephone = ""
        self.website = ""

    def to_dict(self):
        return {self.name: {"address":self.address,"telephone":self.telephone,"website":self.website}}


restaurant = Restaurant("x")
restaurant.address = "y"
restaurant.telephone = "z"
restaurant.website = "w"
restaurant.to_dict()

request = requests.get("http://www.yellowpages.com/search?search_terms=restaurant&geo_location_terms=Pullman%2C%20WA&page=2")
soup = BeautifulSoup(request.content)
info_divs = soup.findAll("div",{"class":"info"})

for index, div in enumerate(info_divs):
    tag = div.find("a",{"class":"business-name"})
    if tag is not None:
        print "{}: {}".format(index+1,tag.text)



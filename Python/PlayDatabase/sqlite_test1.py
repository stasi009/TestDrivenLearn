
import sqlite3
from tqdm import tqdm
from entities import Car

SampleCars = [Car('Audi', 52642),    
              Car('Mercedes', 57127),    
              Car('Skoda', 9000),    
              Car('Volvo', 29000),    
              Car('Bentley', 350000),    
              Car('Hummer', 41400),    
              Car('Volkswagen', 21600)]

Database = "tests.db"

################################ INSERT
def raw_insert(cars):
    template = "INSERT INTO Cars(Brand,Price) VALUES('{brand}',{price})"

    with sqlite3.connect(Database) as conn:
        for c in tqdm(cars):
            conn.execute(template.format(brand=c.brand,price=c.price))

def placeholder_insert():
    c1 = Car(brand="Hummer",price=12359,model="a1")
    c2 = Car(brand="Volvo",price=9000,model="cz")

    with sqlite3.connect(Database) as conn:
        cursor = conn.cursor() 

        # ?  placeholder
        cursor.execute("INSERT INTO Cars(Brand,Model,Price) VALUES(?,?,?)",
                     (c1.brand,c1.model,c1.price))
        print "newly create id: {}".format(cursor.lastrowid)

        # named placeholder
        cursor.execute("INSERT INTO Cars(Brand,Model,Price) VALUES(:brand,:model,:price)",
                     {'brand':c2.brand,'model':c2.model,'price':c2.price})
        print "newly create id: {}".format(cursor.lastrowid)

def insert_many(self):
    with sqlite3.connect(Database) as conn:
        conn.executemany("INSERT INTO Cars(Brand,Price) VALUES(?,?)",((c.brand,c.price) for c in SampleCars))

################################ Load
def cursor2cars(cursor):
    cars = []
    for row in cursor:
        c = Car(brand=row[1],model=row[2],price=row[3])
        c.id = row[0] # we don't need to cast type, it has already to cast to correct type
        cars.append(c)
    return cars


def load_basic():
    with sqlite3.connect(Database) as conn:
        conn.text_factory = str # by default, it will return unicode, so change it to 'str'
        cursor = conn.execute("SELECT Id,Brand,Model,Price FROM Cars ORDER BY PRICE")
        Car.display(cursor2cars(cursor))

def load_access_col_by_name():
    """
    to access each column in row by name instead of index
    we need to set connection's row_factory to 'sqlite3.Row'
    """
    with sqlite3.connect(Database) as conn:
        conn.text_factory = str # by default, it will return unicode, so change it to 'str'
        conn.row_factory = sqlite3.Row
        
        cursor = conn.execute("SELECT * FROM Cars")
        cars = []
        for row in cursor:
            c = Car(brand=row["Brand"],price=row["Price"],model=row["Model"])
            c.id = row["Id"]
            cars.append(c)

        Car.display(cars)

def load_by_parameter(brand):
    with sqlite3.connect(Database) as conn:
        conn.text_factory = str # by default, it will return unicode, so change it to 'str'
        
        # the real parameter expects a tuple or list, so we have to wrap it into tuple
        # even there is just only one element
        cursor = conn.execute("SELECT Id,Brand,Model,Price FROM Cars WHERE Brand=?",(brand,))
        Car.display(cursor2cars(cursor))

if __name__ == "__main__":
    # Car.display(SampleCars)
    # raw_insert(SampleCars)
    load_by_placeholder("Audi")










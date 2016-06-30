
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
    """
    this method is not recommended
    You shouldn’t assemble your query using Python’s string operations because doing so is insecure
    it makes your program vulnerable to an SQL injection attack
    """
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
        # the second parameter doesn't need to be a concrete list
        # it can be a iterator/generator
        conn.executemany("INSERT INTO Cars(Brand,Price) VALUES(?,?)",((c.brand,c.price) for c in SampleCars))

################################ Load
def cursor2cars(cursor):
    cars = []
    for row in cursor:# each row is a tuple
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
        
        # ?  placeholder
        # the real parameter expects a tuple or list, so we have to wrap it
        # into tuple
        # even there is just only one element
        cursor = conn.execute("SELECT Id,Brand,Model,Price FROM Cars WHERE Brand=?",(brand,))
        Car.display(cursor2cars(cursor))

        # named placeholder
        cursor = conn.execute("SELECT COUNT(*) AS Count,AVG(Price) FROM Cars WHERE Brand=:brand",{"brand":brand})
        # we know the result is only one row, so we can call fetchone()
        row = cursor.fetchone()
        print "'{brand}' has {total} cars, average price is {avgprice:.3f}".format(brand=brand,total=row[0],avgprice=row[1])

if __name__ == "__main__":
    # Car.display(SampleCars)
    # raw_insert(SampleCars)
    load_by_parameter("Mercedes")










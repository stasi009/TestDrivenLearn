
import prettytable

class Car(object):
    def __init__(self,brand,price,model=None):
        self.id = -1# primary key, auto increment
        self.brand = brand
        self.price = price
        self.model = model

    @staticmethod
    def display(cars):
        t = prettytable.PrettyTable(["Id","Brand","Model","Price"])
        for c in cars:
            t.add_row((c.id,c.brand,c.model,c.price))
        print t

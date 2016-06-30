
from flask import Flask,url_for,request,render_template
from app import app

@app.route('/')
def hello():
    createLink = "<a href='%s'>Create a question</a>" % url_for('create')
    return """<html>
                   <head>
                       <title>Hello, world!</title>
                    </head>
                    <body>
                       """ + createLink + """
                    </body>
               </html>"""

@app.route('/create', methods=['GET', 'POST'])
def create():
    if request.method == 'GET':
        return render_template('CreateQuestion.html')
    elif request.method == 'POST':# case sensitive
        title = request.form["title"]
        question = request.form["question"]
        answer = request.form["answer"]
        return render_template('QuestionCreated.html',question=question)
    else:
        return "<h2>invalid request</h2>"




#include "shapes.h"
// Window dimensions
const GLuint WIDTH = 800, HEIGHT = 600;

const char* VertexShaderSource = R"(
	        #version 330 core
	        in vec3 coord;
	        in vec3 colorVertex; //Цвет вершины (для градиента)
            uniform mat4 matr;
	        out vec3 vertexColor; //Передача цвета в фрагментный шейдер


	        void main() {
		        gl_Position = matr *  vec4(coord, 1.0f);
                vertexColor = colorVertex;
                
		       
	        }
        )";

const char* FragmentGradientShaderSource = R"(
	        #version 330 core
            out vec4 color;
            in vec3 vertexColor;
            void main()
            {
                color = vec4(vertexColor, 1.0);
            }
        )";
//projection* view* model* vec4(coord, 1.0f);





const std::vector<GLfloat> cube{
    -1.0f,   1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
     1.0f,   1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
     1.0f,  -1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
     -1.0f, -1.0f, 1.0f, 1.0f, 0.0f, 1.0f,


        -1.0f, 1.0f, -1.0f, 0.5f, 0.5f, 0.5f,
        -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f,
        -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f,

        1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
        -1.0f, -1.0f, 1.0f, 0.0f, 0.0f, 1.0f,
        -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f,
        1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f,

        1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
        1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,
        1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f, 
        1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 1.0f,

        1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f, 
        -1.0f, 1.0f, -1.0f, 0.5f, 0.5f, 0.5f, 
        -1.0f, -1.0f, -1.0f, 1.0f, 0.0f, 1.0f, 
        1.0f, -1.0f, -1.0f, 0.0f, 1.0f, 1.0f,

        1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f,
        -1.0f, 1.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        -1.0f, 1.0f, -1.0f, 0.5f, 0.5f, 0.5f,
        1.0f, 1.0f, -1.0f, 1.0f, 1.0f, 1.0f,

};
     





// Компиляция шейдера
GLuint compileShader(const GLchar* source, GLenum type) {
    GLuint shader = glCreateShader(type);
    glShaderSource(shader, 1, &source, nullptr);
    glCompileShader(shader);

    GLint success;
    glGetShaderiv(shader, GL_COMPILE_STATUS, &success);
    if (!success) {
        char infoLog[512];
        glGetShaderInfoLog(shader, 512, nullptr, infoLog);
        std::cerr << "ERROR::SHADER::COMPILATION_FAILED\n" << infoLog << std::endl;
    }

    return shader;
}

// Создание шейдера
GLuint createShaderProgram(int filename) {
    
    //Вертексный шейдер и проверка на ошибку создания
    GLuint vertexShader = compileShader(VertexShaderSource, GL_VERTEX_SHADER);
    
    GLint success;
    GLchar infolog[512];
    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
    if (!success) 
    {
        glGetShaderInfoLog(vertexShader, 512, nullptr, infolog);
        std::cout << "Error compiling vertex shader: " << infolog << std::endl;
    }

    //Фрагментный шейдер и проверка на ошибку создания
    GLuint fragmentShader = compileShader(FragmentGradientShaderSource, GL_FRAGMENT_SHADER);
        
    glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
    if (!success) 
    {
        glGetShaderInfoLog(vertexShader, 512, nullptr, infolog);
        std::cout << "Error compiling fragment shader: " << infolog << std::endl;
    }

    GLuint shaderProgram = glCreateProgram();
    glAttachShader(shaderProgram, vertexShader);
    glAttachShader(shaderProgram, fragmentShader);
    glLinkProgram(shaderProgram);

    return shaderProgram;
}

// Создает фигуру в VBO по вершинам
void getShape(GLuint& VBO, GLuint count) 
{
 
    glGenBuffers(1, &VBO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);

    glBufferData(GL_ARRAY_BUFFER, cube.size() *  sizeof(GLfloat), cube.data(), GL_STATIC_DRAW);
    
    glBindBuffer(GL_ARRAY_BUFFER, NULL);
}

int main() 
{
    sf::Clock clock; 
    sf::RenderWindow window(sf::VideoMode(WIDTH, HEIGHT), "Window");
    glewInit();

    GLuint VBO;
    GLuint progID = 0;
    glEnable(GL_DEPTH_TEST);     //Включаем проверку глубины
    GLuint shaderProgram = createShaderProgram(progID);

    // Вытягиваем ID атрибута вершин из собранной программы
    GLuint Attrib_vertex = glGetAttribLocation(shaderProgram, "coord");
    GLuint Color_vertex = glGetAttribLocation(shaderProgram, "colorVertex");
    getShape(VBO, progID);
    glm::mat4 model = glm::mat4(1.0f);
    model = glm::translate(model, glm::vec3(0.0f, 0.0f, 0.0f));

    glm::mat4 view = glm::lookAt(
        glm::vec3(5.0f, 5.0f, 5.0f),//Положение 
        glm::vec3(0.0f, 0.0f, 0.0f),//Куда должна быть направлена камера
        glm::vec3(0.0f, 1.0f, 0.0f) //Для ориентации
    );


    glm::mat4 projection = glm::perspective(glm::radians(45.0f), (GLfloat)WIDTH / (GLfloat)HEIGHT, 0.1f, 100.0f);

    glm::mat4 mvp = projection * view * model;


    while (window.isOpen()) 
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed) { window.close(); }
            else if (event.type == sf::Event::MouseButtonPressed) 
            {               
                shaderProgram = createShaderProgram(progID);         
                    getShape(VBO, shaderProgram);
            }
        }
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        float time = clock.getElapsedTime().asSeconds();
        glUseProgram(shaderProgram);
        glBindBuffer(GL_ARRAY_BUFFER, VBO);
        //Получаем расположение юниформ-переменных в Вертексном шейдере
        GLuint matrloc = glGetUniformLocation(shaderProgram, "matr");

        //Передаём юниформ-переменные в шейдеры
        glUniformMatrix4fv(matrloc, 1, GL_FALSE, glm::value_ptr(mvp));


        // Считываем координаты x, y, z и передаем по индексу 0
        glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (void*)0);
        glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов

        glVertexAttribPointer(Color_vertex, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
        glEnableVertexAttribArray(Color_vertex);

        glBindBuffer(GL_ARRAY_BUFFER, 0);
        //Куб
        glDrawArrays(GL_QUADS, 0, 24);
        glDisableVertexAttribArray(Attrib_vertex);
        glDisableVertexAttribArray(Color_vertex);
        //glBindVertexArray(0);

        // Считываем цвета r, g, b и передаем по индексу 1
        //glVertexAttribPointer(1, 1, GL_FLOAT, GL_FALSE, 4 * sizeof(GLfloat), (void*)(1 * sizeof(GLfloat))); 
        //glEnableVertexAttribArray(1);

        // uniform - для любых двух шейдеров
        // attrib - для вершинного

        window.display();
    }

    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);
   
    return 0;
}




//Vertex cubeVertices2[] = {  // Вершины кубика
// { -0.5, -0.5, +0.5}, { -0.5, +0.5, +0.5 }, { +0.5, +0.5, +0.5 },
// { +0.5, +0.5, +0.5 }, { +0.5, -0.5, +0.5 }, { -0.5, -0.5, +0.5 },
// { -0.5, -0.5, -0.5 }, { +0.5, +0.5, -0.5 }, { -0.5, +0.5, -0.5 },
// { +0.5, +0.5, -0.5 }, { -0.5, -0.5, -0.5 }, { +0.5, -0.5, -0.5 },
// { -0.5, +0.5, -0.5 }, { -0.5, +0.5, +0.5 }, { +0.5, +0.5, +0.5 },
// { +0.5, +0.5, +0.5 }, { +0.5, +0.5, -0.5 }, { -0.5, +0.5, -0.5 },
// { -0.5, -0.5, -0.5 }, { +0.5, -0.5, +0.5 }, { -0.5, -0.5, +0.5 },
// { +0.5, -0.5, +0.5 }, { -0.5, -0.5, -0.5 }, { +0.5, -0.5, -0.5 },
// { +0.5, -0.5, -0.5 }, { +0.5, -0.5, +0.5 }, { +0.5, +0.5, +0.5 },
// { +0.5, +0.5, +0.5 }, { +0.5, +0.5, -0.5 }, { +0.5, -0.5, -0.5 },
// { -0.5, -0.5, -0.5 }, { -0.5, +0.5, +0.5 }, { -0.5, -0.5, +0.5 },
// { -0.5, +0.5, +0.5 }, { -0.5, -0.5, -0.5 }, { -0.5, +0.5, -0.5 },
//};

//GLfloat vertices[] = {
//       -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
//        0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
//        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
//        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
//       -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
//       -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
//
//       -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
//        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
//        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
//        0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
//       -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
//       -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
//
//       -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
//       -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
//       -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
//       -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
//       -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
//       -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
//
//        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
//        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
//        0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
//        0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
//        0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
//        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
//
//       -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
//        0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
//        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
//        0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
//       -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
//       -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
//
//       -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
//        0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
//        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
//        0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
//       -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
//       -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
//};
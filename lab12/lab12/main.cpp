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
            in vec3 vertexColor;
            in vec2 TexCoord;            
            
            out vec4 color;

            void main()
            {
                color = vec4(vertexColor, 1.0);
            }
        )";


const char* VertexTexShaderSource = R"(
	        #version 330 core
	        layout (location = 0) in vec3 coord;
	        layout (location = 1) in vec3 colorVertex; //Цвет вершины (для градиента)
            layout (location = 2) in vec2 texCoord;
            uniform mat4 matr;
            out vec2 TexCoord;
            out vec3 Coord;


	        void main() {
		        gl_Position = matr *  vec4(coord, 1.0f);
                TexCoord = texCoord;
                Coord = coord;	       
	        }
        )";

const char* FragmentGradientTextureShaderSource = R"(
        #version 330 core
        in vec3 Color;
        in vec2 TexCoord; 
        out vec4 color;
        uniform sampler2D ourTexture;
        void main()
        {
            color =  texture(ourTexture, TexCoord);
        }


)";
//projection* view* model* vec4(coord, 1.0f);



     



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
    GLuint vertexShader;
    if (filename == 1) 
    {
        //Вертексный шейдер и проверка на ошибку создания
        vertexShader = compileShader(VertexTexShaderSource, GL_VERTEX_SHADER);

    }
    else 
    {
        vertexShader = compileShader(VertexShaderSource, GL_VERTEX_SHADER);
    }
    GLint success;
    GLchar infolog[512];
    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
    if (!success) 
    {
        glGetShaderInfoLog(vertexShader, 512, nullptr, infolog);
        std::cout << "Error compiling vertex shader: " << infolog << std::endl;
    }
    GLuint fragmentShader;
    if (filename == 1)
    {
        //Фрагментный шейдер и проверка на ошибку создания
        fragmentShader = compileShader(FragmentGradientTextureShaderSource, GL_FRAGMENT_SHADER);
    }
    else if(filename == 0 || filename == 2)
    {
        //Фрагментный шейдер и проверка на ошибку создания
        fragmentShader = compileShader(FragmentGradientShaderSource, GL_FRAGMENT_SHADER);
    }
        
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
    if (count == 0 || count == 1) 
    {
        glBufferData(GL_ARRAY_BUFFER, cubefigure.size() * sizeof(GLfloat), cubefigure.data(), GL_STATIC_DRAW);
    }
    else if (count == 2) 
    {
        glBufferData(GL_ARRAY_BUFFER, tetrafigure.size() * sizeof(GLfloat), tetrafigure.data(), GL_STATIC_DRAW);
    }

    glBindBuffer(GL_ARRAY_BUFFER, NULL);
}

int main() 
{
    int width, height;
    width = 512; height = 512;
    unsigned char* image = SOIL_load_image("container.jpg", &width, &height, 0, SOIL_LOAD_RGB); //0 - кол-во каналов изображения. SOIL_LOAD_RGB - количество каналов изображения
    
    if(!image) 
    {
        std::cerr << "Failed to load texture" << std::endl;
        return -1;
    }
    //Создадим текстуру
    GLuint texture;
    glGenTextures(1, &texture); //1 - кол-во тестур для генерации
    glBindTexture(GL_TEXTURE_2D, texture);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT); // Повторение текстуры по S
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT); // Повторение текстуры по T
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR); // Линейная фильтрация при уменьшении
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR); // Линейная фильтрация при увеличении
    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, image);

    sf::RenderWindow window(sf::VideoMode(WIDTH, HEIGHT), "Window");
    glewInit();

    GLuint VBO;
    GLuint progID = 0;
    glEnable(GL_DEPTH_TEST);  //Включаем проверку глубины
    GLuint shaderProgram = createShaderProgram(progID);

    // Вытягиваем ID атрибута вершин из собранной программы
    GLuint Attrib_vertex = glGetAttribLocation(shaderProgram, "coord");
    GLuint Color_vertex = glGetAttribLocation(shaderProgram, "colorVertex");
    GLuint Tex_Vertex;
    getShape(VBO, progID);
    glm::mat4 model = glm::mat4(1.0f);
    //model = glm::translate(model, glm::vec3(0.0f, 0.0f, 0.0f));

    glm::mat4 view = glm::lookAt(
        glm::vec3(5.0f, 5.0f, -5.0f),//Положение 
        glm::vec3(0.0f, 0.0f, 0.0f),//Куда должна быть направлена камера
        glm::vec3(0.0f, 1.0f, 0.0f) //Для ориентации
    );

    glm::mat4 projection = glm::perspective(glm::radians(45.0f), (GLfloat)WIDTH / (GLfloat)HEIGHT, 0.1f, 100.0f);

    glm::mat4 mvp;
    mvp = projection * view * model;
    GLfloat moveX = 0.0f;
    GLfloat moveY = 0.0f;
    GLuint count = 0;
    while (window.isOpen()) 
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed) { window.close(); }
            else if (event.type == sf::Event::MouseButtonPressed) 
            {               
                count = (count + 1) % 3;
                shaderProgram = createShaderProgram(count); 
                if (count == 1) 
                {
                    Tex_Vertex = glGetAttribLocation(shaderProgram, "texCoord");
                    std::cout << Tex_Vertex;
                }
                    
                    
                getShape(VBO, count);
            }
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Left))
            {
                
                moveX = -0.5f;
                model = glm::translate(model, glm::vec3(moveX, 0.0f, 0.0f));

            }
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Right)) 
            {
                moveX = 0.5f;
                model = glm::translate(model, glm::vec3(moveX, 0.0f, 0.0f));
                
            }
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Up)) 
            {
                moveY = 0.5f;
                model = glm::translate(model, glm::vec3(0.0f, moveY, 0.0f));
            }
            if (sf::Keyboard::isKeyPressed(sf::Keyboard::Down))
            {
                moveY = -0.5f;
                model = glm::translate(model, glm::vec3(0.0f, moveY, 0.0f));
            }
        }
        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        //float time = clock.getElapsedTime().asSeconds();
        mvp = projection * view * model;
        glUseProgram(shaderProgram);
        glBindBuffer(GL_ARRAY_BUFFER, VBO);
        //Получаем расположение юниформ-переменных в Вертексном шейдере
        GLuint matrloc = glGetUniformLocation(shaderProgram, "matr");

        //Передаём юниформ-переменные в шейдеры
        glUniformMatrix4fv(matrloc, 1, GL_FALSE, glm::value_ptr(mvp));

        if (count == 0) 
        {
            // Считываем координаты x, y, z и передаем по индексу 0
            glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)0);
            glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов

            glVertexAttribPointer(Color_vertex, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Color_vertex);
        }
        if (count == 1) 
        {
            // Считываем координаты x, y, z и передаем по индексу 0
            glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)0);
            glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов

            glVertexAttribPointer(Color_vertex, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Color_vertex);

            glVertexAttribPointer(Tex_Vertex, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(6 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Tex_Vertex);
        }
        if (count == 2)
        {
            // Считываем координаты x, y, z и передаем по индексу 0
            glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (void*)0);
            glEnableVertexAttribArray(Attrib_vertex); // Включаем массив атрибутов

            glVertexAttribPointer(Color_vertex, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
            glEnableVertexAttribArray(Color_vertex);

        }
                    
        
        glBindBuffer(GL_ARRAY_BUFFER, 0);
        if (count == 0 || count == 1) 
        {
            glBindTexture(GL_TEXTURE_2D, texture);
            //Куб
            glDrawArrays(GL_QUADS, 0, 24);
        }
        else if(count == 2) 
        {
            //Тетраэдр
            glDrawArrays(GL_TRIANGLES, 0, 12);
        }
        glDisableVertexAttribArray(Attrib_vertex);
        glDisableVertexAttribArray(Color_vertex);
        //glBindVertexArray(0);
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
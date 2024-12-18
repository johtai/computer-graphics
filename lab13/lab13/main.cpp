#include "shapes.h"
// Window dimensions
const GLuint WIDTH = 1200, HEIGHT = 900;


const char* VertexTexShaderSource = R"(
	        #version 330 core
	        layout (location = 0) in vec3 coord;
	        //layout (location = 1) in vec3 normals;
            layout (location = 1) in vec2 texCoord;

            out vec2 TexCoord;
            //out vec3 Normals;

            uniform mat4 matr;

	        void main() {
		        gl_Position = matr *  vec4(coord, 1.0f);
                TexCoord = texCoord;
                //Normals = normals;
                      
	        }
        )";

const char* FragmentGradientTextureShaderSource = R"(
        #version 330 core
        in vec2 TexCoord; 
        //in vec3 Normals;

        out vec4 color;

        uniform sampler2D ourTexture;
        void main()
        {
            color =  texture(ourTexture, TexCoord);
        }


)";

// Компиляция шейдера
GLuint compileShader(const GLchar* source, GLenum type) 
{
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


GLuint loadTexture(const char* filePath) 
{
    int width, height;
    unsigned char* image = SOIL_load_image(filePath, &width, &height, 0, SOIL_LOAD_RGB);
    if (!image) {
        std::cerr << "Failed to load texture" << std::endl;
        return 0;
    }

    GLuint texture;
    glGenTextures(1, &texture);
    glBindTexture(GL_TEXTURE_2D, texture);

    glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
    glGenerateMipmap(GL_TEXTURE_2D);

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

    SOIL_free_image_data(image);
    glBindTexture(GL_TEXTURE_2D, 0);

    return texture;
}

// Создание шейдера
GLuint createShaderProgram(int filename) 
{  
    GLuint vertexShader = compileShader(VertexTexShaderSource, GL_VERTEX_SHADER);
    
    GLint success;
    GLchar infolog[512];
    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
    if (!success) 
    {
        glGetShaderInfoLog(vertexShader, 512, nullptr, infolog);
        std::cout << "Error compiling vertex shader: " << infolog << std::endl;
    }

    GLuint fragmentShader = compileShader(FragmentGradientTextureShaderSource, GL_FRAGMENT_SHADER);
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


void setupVAO(GLuint& VAO, GLuint& VBO, const std::vector <GLfloat>& vertices) 
{
    glGenVertexArrays(1, &VAO);
    glGenBuffers(1, &VBO);

    glBindVertexArray(VAO);

    glBindBuffer(GL_ARRAY_BUFFER, VBO);
    glBufferData(GL_ARRAY_BUFFER, vertices.size() * sizeof(GLfloat), vertices.data(), GL_STATIC_DRAW);

    // Position attribute
    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)0);
    glEnableVertexAttribArray(0);

    // Color attribute
    //glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(3 * sizeof(GLfloat)));
    //glEnableVertexAttribArray(1);

    // Texture coordinate attribute
    glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GLfloat), (void*)(6 * sizeof(GLfloat)));
    glEnableVertexAttribArray(1);

    glBindBuffer(GL_ARRAY_BUFFER, 0);
    glBindVertexArray(0);

}

struct Camera 
{
    glm::vec3 position;
    glm::vec3 front; 
    glm::vec3 up;
    glm::vec3 right;

    float yaw; // Горизонтальный угол поворота (в градусах)
    float pitch;// Вертикальный угол поворота (в градусах)
    float fov; // Угол обзора (в градусах)

    Camera(glm::vec3 startPosition) 
        : position(startPosition),
        front(glm::vec3(0.0f, 0.0f, -1.0f)),
        up(glm::vec3(0.0f, 1.0f, 0.0f)),
        yaw(-90.0f), pitch(0.0f), fov(45.0f){}
};


glm::mat4 getViewMatrix(const Camera& camera) 
{
    return glm::lookAt(camera.position, camera.position + camera.front, camera.up);
}



void updateCameraPosition(Camera& camera, float deltaTime, const sf::Keyboard keyboard)
{
    float speed = 2.5f * deltaTime; //Скорость движения
}

glm::mat4 getProjectionMatrix(float aspectRatio, const Camera& camera) {
    return glm::perspective(glm::radians(camera.fov), aspectRatio, 0.1f, 100.0f);
}


void rotateCamera(Camera& camera, float deltaTime) {
    float rotationSpeed = 2.5f * deltaTime; // Скорость вращения в градусах

    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Q)) {
        camera.yaw -= rotationSpeed; // Поворот камеры влево
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::E)) {
        camera.yaw += rotationSpeed; // Поворот камеры вправо
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::R)) {
        camera.pitch += rotationSpeed; // Поворот камеры вверх
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::F)) {
        camera.pitch -= rotationSpeed; // Поворот камеры вниз
    }

    // Ограничение вертикального поворота
    if (camera.pitch > 89.0f) camera.pitch = 89.0f;
    if (camera.pitch < -89.0f) camera.pitch = -89.0f;

    // Пересчёт направления камеры
    glm::vec3 front;
    front.x = cos(glm::radians(camera.yaw)) * cos(glm::radians(camera.pitch));
    front.y = sin(glm::radians(camera.pitch));
    front.z = sin(glm::radians(camera.yaw)) * cos(glm::radians(camera.pitch));
    camera.front = glm::normalize(front);

    // Пересчёт векторов "вправо" и "вверх"
    camera.right = glm::normalize(glm::cross(camera.front, glm::vec3(0.0f, 1.0f, 0.0f)));
    camera.up = glm::normalize(glm::cross(camera.right, camera.front));
}

void moveCamera(Camera& camera, float deltaTime) {
    //float speed = 2.5f * deltaTime; // Скорость движения
    float speed = 10.0f * deltaTime;
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::W)) {
        camera.position += speed * camera.front; // Вперёд
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::S)) {
        camera.position -= speed * camera.front; // Назад
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::A)) {
        camera.position -= speed * camera.right; // Влево
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::D)) {
        camera.position += speed * camera.right; // Вправо
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Space)) {
        camera.position += speed * camera.up;    // Вверх
    }
    if (sf::Keyboard::isKeyPressed(sf::Keyboard::LShift)) {
        camera.position -= speed * camera.up;    // Вниз
    }
}




int main() 
{
    setlocale(LC_ALL, "ru");
    Camera camera(glm::vec3(0.0f, 0.0f, 3.0f));
    /*std::vector<GLfloat> loadedVertices = ParseObjFromFile("tako.obj");*/
    std::vector<GLfloat> loadedVertices = ParseObjFromFile("lootbox_model.obj");
    int width = 512;
    int height = 512;

    sf::RenderWindow window(sf::VideoMode(WIDTH, HEIGHT), "Window");
    glewInit();


    GLuint progID = 0;

    glEnable(GL_DEPTH_TEST);  //Включаем проверку глубины
    glDepthFunc(GL_LESS);    // Отрисовка ближних объектов
    glEnable(GL_CULL_FACE);          // Включить отсечение граней
    glCullFace(GL_BACK);             // Отсечь задние грани
    glFrontFace(GL_CCW);             // Определить, что передняя грань имеет порядок вершин counter-clockwise (по умолчанию)
    // Если используется прозрачность
    glEnable(GL_BLEND);
    glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

    GLuint shaderProgram = createShaderProgram(progID);

    GLuint VAO, VBO;
    setupVAO(VAO, VBO, loadedVertices);

    //Создадим текстуру
    //GLuint texture = loadTexture("ambientMap1.png");
    GLuint texture = loadTexture("lootbox_model.png");
    if (texture == 0) {
        std::cerr << "Error: Texture not loaded!" << std::endl;
        return -1;
    }


    glm::mat4 model = glm::mat4(1.0f);
    //glm::mat4 view = glm::lookAt(
    //    glm::vec3(1.0f, 1.0f, -1.0f),//Положение 
    //    glm::vec3(0.0f, 0.0f, 0.0f),//Куда должна быть направлена камера
    //    glm::vec3(0.0f, 1.0f, 0.0f) //Для ориентации
    //);

    glm::mat4 projection = glm::perspective(glm::radians(45.0f), (GLfloat)WIDTH / (GLfloat)HEIGHT, 0.1f, 500.0f);

    glm::mat4 mvp;
    //mvp = projection * view * model;
    GLfloat moveX = 0.0f;
    GLfloat moveY = 0.0f;
    GLfloat scaleUp = 0.0f;
    GLuint count = 0;
    float lastTime = 0.0f;
    sf::Clock clock;


    while (window.isOpen()) 
    {
        float currentTime = clock.getElapsedTime().asSeconds(); //получает текущее время в секундах.
        float deltaTime = currentTime - lastTime; // обновляется, чтобы сохранить текущее время для следующего шага.
        lastTime = currentTime; //вычисляет разницу между текущим временем и временем последнего обновления 
      
        sf::Event event;
        // Управление камерой
        while (window.pollEvent(event))
        {

          
            if (event.type == sf::Event::Closed) { window.close(); }
            else if (event.type == sf::Event::MouseButtonPressed) 
            {               
                shaderProgram = createShaderProgram(count);
            }

          

            //Keyboard Actions
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

            if (sf::Keyboard::isKeyPressed(sf::Keyboard::T)) 
            {
                model = glm::rotate(model, glm::radians(45.0f), glm::vec3(0.0f, 1.0f, 0.0f));
            }

        }


        rotateCamera(camera, deltaTime);
        moveCamera(camera, deltaTime);
        glm::mat4 view = getViewMatrix(camera);


        glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
        mvp = projection * view * model;
        glUseProgram(shaderProgram);
        //Получаем расположение юниформ-переменных в Вертексном шейдере
        GLuint matrloc = glGetUniformLocation(shaderProgram, "matr");

        //Передаём юниформ-переменные в шейдеры
        glUniformMatrix4fv(matrloc, 1, GL_FALSE, glm::value_ptr(mvp));                   
        
        glBindBuffer(GL_ARRAY_BUFFER, 0);
        glBindTexture(GL_TEXTURE_2D, texture);
        //Куб
        glBindVertexArray(VAO);
        glDrawArrays(GL_TRIANGLES, 0, loadedVertices.size());
        glBindVertexArray(0);
        //GL_QUADS


        window.display();
    }
    glDeleteVertexArrays(1, &VAO);
    glDeleteBuffers(1, &VBO);
    glUseProgram(0);
    glDeleteProgram(shaderProgram);
   
    return 0;
}





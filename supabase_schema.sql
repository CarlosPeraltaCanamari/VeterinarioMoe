-- ==============================================================================
-- SISTEMA DE GESTIÓN CLÍNICA VETERINARIA "EL ARCA DE MOE"
-- SCRIPT DDL / DML PARA SUPABASE (POSTGRESQL)
-- ==============================================================================

-- 1. TABLA: PROPIETARIOS
CREATE TABLE IF NOT EXISTS public.propietarios (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(60) NOT NULL,
    apellidos VARCHAR(80) NOT NULL,
    telefono VARCHAR(20) NOT NULL,
    email VARCHAR(100) NOT NULL,
    direccion VARCHAR(200),
    estado BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_registro TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_propietarios_email ON public.propietarios(email);
CREATE INDEX IF NOT EXISTS idx_propietarios_telefono ON public.propietarios(telefono);
CREATE INDEX IF NOT EXISTS idx_propietarios_estado ON public.propietarios(estado);

-- 2. TABLA: MASCOTAS
CREATE TABLE IF NOT EXISTS public.mascotas (
    id SERIAL PRIMARY KEY,
    propietario_id INT NOT NULL,
    nombre VARCHAR(50) NOT NULL,
    especie VARCHAR(40) NOT NULL,
    raza VARCHAR(60) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    sexo VARCHAR(10) NOT NULL DEFAULT 'Macho' CHECK (sexo IN ('Macho', 'Hembra')),
    peso_kg NUMERIC(5,2),
    estado BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_registro TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT fk_mascotas_propietario FOREIGN KEY (propietario_id) 
        REFERENCES public.propietarios(id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS idx_mascotas_propietario_id ON public.mascotas(propietario_id);
CREATE INDEX IF NOT EXISTS idx_mascotas_nombre ON public.mascotas(nombre);
CREATE INDEX IF NOT EXISTS idx_mascotas_estado ON public.mascotas(estado);

-- 3. TABLA: VETERINARIOS
CREATE TABLE IF NOT EXISTS public.veterinarios (
    id SERIAL PRIMARY KEY,
    nombre VARCHAR(60) NOT NULL,
    apellidos VARCHAR(80) NOT NULL,
    especialidad VARCHAR(80) NOT NULL,
    telefono VARCHAR(20) NOT NULL,
    email VARCHAR(100),
    estado BOOLEAN NOT NULL DEFAULT TRUE,
    fecha_registro TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_veterinarios_especialidad ON public.veterinarios(especialidad);
CREATE INDEX IF NOT EXISTS idx_veterinarios_estado ON public.veterinarios(estado);

-- 4. TABLA: CITAS Y DIAGNÓSTICOS
CREATE TABLE IF NOT EXISTS public.citas (
    id SERIAL PRIMARY KEY,
    mascota_id INT NOT NULL,
    veterinario_id INT NOT NULL,
    fecha_hora TIMESTAMPTZ NOT NULL,
    motivo VARCHAR(250) NOT NULL,
    estado VARCHAR(20) NOT NULL DEFAULT 'Pendiente' CHECK (estado IN ('Pendiente', 'Completada', 'Cancelada')),
    diagnostico TEXT,
    tratamiento TEXT,
    fecha_creacion TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    CONSTRAINT fk_citas_mascota FOREIGN KEY (mascota_id) 
        REFERENCES public.mascotas(id) ON DELETE RESTRICT,
    CONSTRAINT fk_citas_veterinario FOREIGN KEY (veterinario_id) 
        REFERENCES public.veterinarios(id) ON DELETE RESTRICT
);

CREATE INDEX IF NOT EXISTS idx_citas_fecha_hora ON public.citas(fecha_hora);
CREATE INDEX IF NOT EXISTS idx_citas_estado ON public.citas(estado);
CREATE INDEX IF NOT EXISTS idx_citas_mascota_id ON public.citas(mascota_id);
CREATE INDEX IF NOT EXISTS idx_citas_veterinario_id ON public.citas(veterinario_id);

-- 5. TABLA: USUARIOS (SEGURIDAD Y CONTROL DE ACCESO)
CREATE TABLE IF NOT EXISTS public.usuarios (
    id SERIAL PRIMARY KEY,
    nombre_completo VARCHAR(100) NOT NULL,
    email VARCHAR(120) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    rol VARCHAR(30) NOT NULL CHECK (rol IN ('Administrador', 'Veterinario', 'Recepcionista')),
    estado BOOLEAN NOT NULL DEFAULT TRUE,
    ultimo_acceso TIMESTAMPTZ,
    fecha_registro TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE UNIQUE INDEX IF NOT EXISTS idx_usuarios_email ON public.usuarios(email);

-- ==============================================================================
-- 6. DATOS SEMILLA DE PRUEBA (INSERT INICIAL)
-- ==============================================================================

-- Propietarios de muestra
INSERT INTO public.propietarios (id, nombre, apellidos, telefono, email, direccion, estado)
VALUES 
(1, 'Homero', 'Simpson', '+56 9 1111 2222', 'homero@springfield.com', 'Av. Siempre Viva 742', TRUE),
(2, 'Ned', 'Flanders', '+56 9 3333 4444', 'ned@flanders.com', 'Av. Siempre Viva 744', TRUE),
(3, 'Patricia', 'Bouvier', '+56 9 5555 6666', 'patricia.b@springfield.com', 'Calle Elm 123', TRUE)
ON CONFLICT (id) DO NOTHING;

-- Mascotas de muestra
INSERT INTO public.mascotas (id, propietario_id, nombre, especie, raza, fecha_nacimiento, sexo, peso_kg, estado)
VALUES 
(1, 1, 'Ayudante de Santa', 'Canino', 'Galgo Inglés', CURRENT_DATE - INTERVAL '3 years', 'Macho', 22.50, TRUE),
(2, 1, 'Bola de Nieve V', 'Felino', 'Persa Mestizo', CURRENT_DATE - INTERVAL '2 years', 'Hembra', 4.20, TRUE),
(3, 2, 'Barty', 'Canino', 'Golden Retriever', CURRENT_DATE - INTERVAL '4 years', 'Macho', 31.00, TRUE)
ON CONFLICT (id) DO NOTHING;

-- Veterinarios de muestra
INSERT INTO public.veterinarios (id, nombre, apellidos, especialidad, telefono, email, estado)
VALUES 
(1, 'Moe', 'Szyslak', 'Cirugía y Traumatología', '+56 9 8765 4321', 'moe@elarcademoe.com', TRUE),
(2, 'Lisa', 'Simpson', 'Medicina Felina y Exóticos', '+56 9 8123 4567', 'lisa.simpson@elarcademoe.com', TRUE),
(3, 'Carlos', 'Mendoza', 'Medicina General y Preventiva', '+56 9 7654 3210', 'cmendoza@elarcademoe.com', TRUE)
ON CONFLICT (id) DO NOTHING;

-- Citas de muestra
INSERT INTO public.citas (id, mascota_id, veterinario_id, fecha_hora, motivo, estado, diagnostico, tratamiento)
VALUES 
(1, 1, 1, NOW() + INTERVAL '2 hours', 'Revisión posoperatoria de extremidad posterior derecha', 'Pendiente', NULL, NULL),
(2, 2, 2, NOW() - INTERVAL '2 days', 'Vacunación anual trivalente felina y desparasitación', 'Completada', 'Paciente clínicamente sano. Mucosas rosadas, ganglios normales, sin pulgas ni parásitos visibles.', 'Se administra vacuna Felocell 3 y antiparasitario interno en comprimido masticable.'),
(3, 3, 1, NOW() + INTERVAL '1 day', 'Molestia en oreja izquierda, rascado frecuente y sacudidas', 'Pendiente', NULL, NULL)
ON CONFLICT (id) DO NOTHING;

-- Sincronización de secuencias seriales en PostgreSQL
SELECT setval('public.propietarios_id_seq', COALESCE((SELECT MAX(id) FROM public.propietarios), 1));
SELECT setval('public.mascotas_id_seq', COALESCE((SELECT MAX(id) FROM public.mascotas), 1));
SELECT setval('public.veterinarios_id_seq', COALESCE((SELECT MAX(id) FROM public.veterinarios), 1));
SELECT setval('public.citas_id_seq', COALESCE((SELECT MAX(id) FROM public.citas), 1));
SELECT setval('public.usuarios_id_seq', COALESCE((SELECT MAX(id) FROM public.usuarios), 1));

-- Confirmación de ejecución exitosa
SELECT 'Tablas e índices de El Arca de Moe creados exitosamente en Supabase' AS resultado;

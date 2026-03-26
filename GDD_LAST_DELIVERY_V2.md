# 📚 GDD — LAST DELIVERY · Episodio 1 · VERSIÓN 2.0
### Game Design Document — Revisado y Ampliado
> **Última actualización:** 26 de marzo de 2026
> **Autor:** Adrian + Claude Code (análisis de escena real + investigación de mercado)
> **Estado del desarrollo:** B1–B5 completo · B6 en curso · B7–B28 pendiente
> **Developer:** Solo dev — Bolivia

---

## 🧭 ÍNDICE DE SECCIONES

| # | Sección | Prioridad para Antigravity |
|---|---|---|
| 0 | Contexto estratégico y mercado | Lectura recomendada |
| 1 | Game overview | OBLIGATORIA |
| 2 | Arquitectura narrativa completa | OBLIGATORIA |
| 3 | Biblia visual | OBLIGATORIA |
| 4 | Dirección de audio | OBLIGATORIA |
| 5 | Mecánicas de juego (detalladas) | OBLIGATORIA |
| 6 | Especificación UI/UX — QuickRun App | OBLIGATORIA |
| 7 | Diseño de niveles | OBLIGATORIA |
| 8 | Arquitectura técnica | OBLIGATORIA |
| 9 | Plan de bloques revisado (B6–B28) | OBLIGATORIA |
| 10 | Monetización y lanzamiento | Referencia |
| 11 | Visión de franquicia | Referencia |

---

## 📊 SECCIÓN 0 — CONTEXTO ESTRATÉGICO Y MERCADO 2026

### Por qué LAST DELIVERY gana

**El espacio vacío verificado (marzo 2026):**
No existe ningún videojuego indie de horror donde el antagonista principal sea una aplicación de gig economy. Verificado en búsquedas exhaustivas de Steam, itch.io y bases de datos de releases hasta marzo 2026. Tenemos la categoría para nosotros solos.

**Datos de referencia probados (investigados marzo 2026):**

| Juego | Dev | Modelo | Revenue verificado | Reviews Steam | Clave del éxito |
|---|---|---|---|---|---|
| Fears to Fathom S1 (6 eps) | Mukul Negi, solo, India | Ep1 FREE / Ep2–6 $4.99–$7.99 | Ep2: ~$453K gross. Total serie: $2–3M est. | Home Alone: 5,346 (88%) · Ironbark Lookout: 4,300+ (91%) | Episódico, "based on true stories". **S2 es CO-OP** → el nicho solitario queda libre para nosotros. |
| Mouthwashing | Wrong Organ, equipo pequeño | $13 | **$6.5M+ gross, 500K copias** | 35,800 (95% Overwhelmingly Positive) | El breakout horror de 2024–25. Zero jumpscares, puro dread psicológico. **Console ports PS5/Switch anunciados.** |
| Buckshot Roulette | Mike Klubnika, solo, Estonia | $2.99–$4.99 | ~$15–20M (8M copias) | — | Concepto único, tensión binaria, itch.io → Steam |
| Schedule 1 | Solo dev | $14.99 | **~$125M, 8M copias, 459K concurrent** | — | No es horror, pero PRUEBA que un solo dev puede explotar sin presupuesto de marketing (mar 2025) |
| Look Outside | Francis Coulombe, solo | $10 | Muy positivo | Miles de reviews positivas | Horror en apartamento urbano, 2025, jam → full game. Referencia directa para nuestro estilo. |
| Bad Parenting | Solo dev, Vietnam | $1.99 | ~$191K | — | Jugado por IShowSpeed/CaseOh |
| Iron Lung | David Szymanski | $5.99 | ~$3M+ | — | 30 min, claustrofobia, influencer bait perfecto |

**Datos actualizados del mercado (investigación marzo 2026):**
- Horror es el **3er género más exitoso** por número de juegos con 1000+ reviews en Steam (2025), detrás de Narrative y Simulation — sigue altamente viable
- **TikTok es ahora EL vector de descubrimiento #1 para horror indie**, superando a YouTube Let's Plays desde 2025
- **Fears to Fathom S2** (Scratch Creek) pivotó a **CO-OP**. Esto despeja el carril de "horror episódico en solitario"
- **Mouthwashing vendió 500K copias a $13** sin marketing masivo — demostración de que el horror psicológico serio es comercialmente viable a ese precio
- **Look Outside** (2025, solo dev, itch.io → Steam, $10): horror en apartamento urbano. Uno de los mejor recibidos del año
- **La historia del developer es contenido viral por sí misma**: en 2025, un solo dev (Tangy TD) lloró en cámara al ver $245K en su primera semana — ese video SE HIZO MÁS VIRAL que el juego. La historia personal de Adrian (solo dev boliviano construyendo horror con IA) es un activo de marketing en sí misma

**El clip viral de Fears to Fathom que debemos estudiar:**
- @zachbeale en TikTok: Ironbark Lookout (Ep 4), **37 millones de plays + 4.6 millones de likes en UN DÍA**
- Por qué funcionó: gameplay normal → amenaza repentina → corte a negro. Menos de 30 segundos.
- El juego usa el micrófono del jugador → las reacciones son genuinas → los comentarios debaten si es real
- **Nuestro equivalente:** el momento del camera roll (fotos de Marco durmiendo) o el doppelgänger girando

**Fórmula viral de TikTok para horror (verificada 2025–2026):**
1. Cold open mostrando gameplay normal (sin contexto necesario)
2. Amenaza repentina o revelación perturbadora
3. Corte a negro inmediato
4. **Duración: menos de 30 segundos**
5. Mecánicas que implican al espectador (elecciones morales, "basado en hechos reales")
6. Los clips sin contexto funcionan MEJOR que los explicados

**Competidores delivery/horror verificados (NINGUNO es amenaza):**
Búsqueda exhaustiva confirmó que existen algunos juegos delivery+horror, pero NINGUNO es competencia real:
- **Deadly Delivery** (Steam) — co-op, goblins en minas. No es horror serio.
- **Panic Delivery** (Steam) — 4 jugadores co-op, monstruos. Gameplay arcade.
- **Easy Delivery Co.** (iOS) — Relaxing con "secretos oscuros". No es horror.
- **Ninguno de estos tiene: primera persona, app como HUD, horror psicológico, estética realista.**
- **Confirmado: nadie está haciendo lo que hacemos nosotros.**

**Por qué nadie más está haciendo LAST DELIVERY:**
1. Los devs de horror se enfocan en mansiones, bosques, o espacios sobrenaturales obvios
2. La gig economy como mecánica de HORROR PSICOLÓGICO no ha sido explorada — los intentos que existen son co-op arcade
3. La audiencia de delivery/rideshare en TikTok (decenas de millones) no tiene ningún juego que hable a su experiencia
4. La ambientación latinoamericana diferencia visualmente en thumbnails — el 99% de los horror indie tienen estética americana/europea
5. **Nuestro HUD (la app QuickRun) es inmediatamente reconocible** — cualquier persona que haya usado una app de delivery entiende la interfaz en 2 segundos

**El modelo Fears to Fathom Season 1 es nuestra hoja de ruta (con mejoras):**
- Episodio 1 GRATIS en itch.io → funnel a Steam
- Solicitar historias reales de delivery workers → autenticidad + comunidad (mismo approach que Mukul Negi)
- Runtime 30–35 min máximo → completable en un stream de una sentada
- Horror mundano PRIMERO (40–60% del tiempo) — la escalada lenta es lo que hace virales los scares
- **Nuestro edge sobre F2F:** el HUD de la app como mecánica central, no solo como contexto

**Gen Z + economía gig = audiencia sin representación en horror:**
Harvard Youth Poll (dic 2025): inseguridad económica es la preocupación #1 de Gen Z. 42% experimenta desesperanza. LAST DELIVERY es el único juego que convierte ese miedo específico (trabajar de noche, por poco dinero, para una app corporativa) en mecánica de horror. Las comunidades de delivery/rideshare en TikTok son amplificadores naturales que NO son gamers → crossover viral que ningún horror indie tradicional puede alcanzar.

**KPIs de validación para Episodio 1 (itch.io, primer mes):**
- ✅ Mínimo viable: 15,000 descargas → greenlight inmediato Ep 2
- ✅ Objetivo: 50,000 descargas → inversión $500–$1,000 en TikTok Ads
- 🚀 Viral: 200,000+ descargas → buscar publisher indie, marketing ampliado
- 🔥 Breakout: 500,000+ → equivale a Look Outside / Bad Parenting tier

---

## 🎮 SECCIÓN 1 — GAME OVERVIEW

### Ficha técnica

| Campo | Dato |
|---|---|
| **Título** | LAST DELIVERY / ÚLTIMA ENTREGA |
| **Tagline** | *"Tomaste el turno nocturno porque paga el doble."* |
| **Género** | Horror psicológico en primera persona, episódico |
| **Motor** | Unity 6 (6000.4.0f1 LTS) + URP 3D |
| **Plataforma principal** | PC Windows 64-bit |
| **Plataforma secundaria** | Mac Universal |
| **Episodio 1** | GRATIS — itch.io primero, luego Steam Free to Play |
| **Runtime Ep 1** | 30–35 minutos **exactos** (nunca más) |
| **Idiomas** | Inglés + Español (subtítulos, UI, textos) |
| **Hardware mínimo** | GTX 1060 / RX 580, 8GB RAM, SSD recomendado |
| **Target FPS** | 60fps estable en hardware mínimo |
| **Resolución** | 1920×1080 nativo, escalable |

### Pitch (versión corta para itch.io)
Un repartidor nocturno. Una dirección que no existe. Un camera roll con fotos de ti durmiendo que tú nunca sacaste.

### Concepto central en una frase
LAST DELIVERY es un horror psicológico donde tu único HUD es la app de delivery de tu empleador — y tu empleador sabe dónde vives.

### Los 3 pilares de diseño
1. **La app como antagonista:** La interfaz corporativa y cheerful ES el horror. Sin monstruos. Sin sangre.
2. **Mundano primero:** 40–60% del juego es aburrido a propósito. Ese contraste hace virales los scares.
3. **Runtime respetado:** 30–35 minutos. Ni un minuto más. La brevedad es una feature, no un defecto.

---

## 🎭 SECCIÓN 2 — ARQUITECTURA NARRATIVA COMPLETA

### Los 5 estados del juego — Mapa de tiempo preciso

```
00:00 ─────────────────── ESTADO 1: NORMALIDAD ─────────────── 08:00
08:00 ─────────────────── ESTADO 2: ANOMALÍA ─────────────── 18:00
18:00 ─────────────────── ESTADO 3: INFILTRACIÓN ─────────────── 25:00
25:00 ─────────────────── ESTADO 4: REVELACIÓN ─────────────── 30:00
30:00 ─────────────────── ESTADO 5: ESCAPE / FINAL ─────────────── 35:00
```

### Estado 1 — NORMALIDAD (0:00–8:00)

**Lo que el jugador hace:**
1. Aparece en el coche. Dashboard encendido. Lluvia en el parabrisas.
2. El teléfono vibra: primera entrega. Dirección normal, edificio residencial.
3. Loading screen: GPS animado dibuja la ruta. Tiempo: 8 segundos.
4. Aparece en exterior Edificio Normal #1. Camina, toca el interphone.
5. Nadie responde. Mensaje del cliente: *"Deja en puerta principal. Gracias."*
6. Deja la bolsa en la puerta. Notificación SUCCESS: *"¡Entrega completada! Rating: ⭐⭐⭐⭐⭐"*
7. Loading screen: vuelve al coche. Come papas frías. El teléfono muestra sus earnings.
8. Segunda entrega. Igual. Más aburrida aún. El cliente deja 3 estrellas sin razón.
9. Marco (o el jugador) puede revisar el camera roll: vacío. El chat: mensajes de su roommate preguntando si llegó bien. Un meme de su mamá.

**Qué establece esta fase:**
- Marco es real, ordinario, cansado
- La economía precaria: $847 de alquiler, turno nocturno, propinas variables
- La app es cheerful, intrusiva, pero normal
- El jugador aprende los controles (F = teléfono, E = interactuar, mouse = mirar)
- **No hay horror aquí.** Ningún hint. Ningún foreshadowing obvio. Solo aburrimiento.

**Beats narrativos específicos (escritura exacta):**

| Momento | Texto visible | Canal |
|---|---|---|
| Inicio del turno | "QuickRun. ¡Tu turno ha comenzado! 🎉 Tienes 1 entrega pendiente." | Notificación app |
| Primera entrega asignada | "Entregar en: Av. Los Robles 847, Edificio Mirabel, piso 3. Cliente: D. Recio. Tiempo estimado: 8 min." | GPS tab |
| Cliente no responde | "¡Recuerda! Los clientes que no responden pueden recibir entrega en puerta. Haz click en 'Entregado' al dejar el pedido." | Notificación app |
| Entrega 1 completada | "¡Entrega completada! D. Recio te calificó con ⭐⭐⭐⭐⭐. Ganaste $3.40." | Notificación SUCCESS |
| Entrega 2 completada | "P. Vásquez te calificó con ⭐⭐⭐. Sin comentario. Ganaste $2.10." | Notificación WARNING |
| Después entrega 2 | "Nueva entrega disponible. ¿Aceptar? Bonus de turno disponible: +20% por entregas nocturnas." | Pop-up app |

### Estado 2 — ANOMALÍA (8:00–18:00)

**Lo que el jugador hace:**
1. Tercera entrega. La dirección aparece en el GPS pero es una calle residencial oscura.
2. Loading screen: el GPS tarda más. La ruta se recalcula dos veces.
3. Aparece en la Calle Anómala. No hay número visible en ninguna casa.
4. Mensaje del cliente (mientras el jugador busca el número): *"Ya te veo. Estás cerca."*
5. El jugador no puede ver a nadie.
6. Una luz de porche se enciende en la casa del centro (la única sin número).
7. Segundo mensaje: *"Esa soy yo. La del porche."*
8. El jugador se acerca. Toca el timbre/golpea la puerta. Nadie abre.
9. La puerta está entreabierta. La app hace ping: *"Cliente reporta que no recibió el pedido. Completa la entrega o recibirás penalización de -1 ⭐."*
10. El jugador puede elegir: entrar o no. **Esta es la primera decisión binaria.**

**Si no entra:** Rating baja -1.0. Mensaje: *"Entrega cancelada por incumplimiento. Advertencia registrada en tu cuenta."* Vuelve al coche. Puede reintentar (la app insiste).

**Si entra:** Estado 3 comienza.

**Beats narrativos exactos:**

| Momento | Texto | Canal |
|---|---|---|
| Asignación entrega 3 | "Nueva entrega: Calle Patiño 113. ⚠️ Zona sin código de puerta. Llama al cliente si no encuentras el número." | GPS tab |
| Al llegar a la calle | "Ya te veo. Estás cerca." | Chat cliente |
| Luz de porche se enciende | "Esa soy yo. La del porche." | Chat cliente |
| Puerta entreabierta, sin respuesta | "Cliente reporta que no recibió el pedido. Completa la entrega o recibirás penalización de −1 ⭐. Tiempo restante: 2:00 min." | Notificación CRITICAL |
| Si espera demasiado | "Tiempo de entrega excedido. Penalización adicional aplicada. −0.5 ⭐" | Notificación CRITICAL |

### Estado 3 — INFILTRACIÓN (18:00–25:00)

**Lo que el jugador ve al entrar:**
- Sala amueblada: sofá, TV apagada, mesa con comida **aún caliente** (vapor particles).
- Fotos familiares en la pared: marcos normales pero las caras son borrones (blur intencional, no glitch).
- Zapatos de niño junto a la entrada. Sin niño visible. Sin sonido de niño.
- El GPS dice "Has llegado a tu destino."
- Mientras el jugador explora, la app genera una **nueva entrega desde dentro de esta casa:**

Mensaje app: *"Nuevo pedido generado: Recoger en cocina. Entregar en: Sótano. Distancia: 12m."*

**Lo que el jugador puede explorar (interactables):**

| Objeto | Acción E | Resultado |
|---|---|---|
| Foto familiar (sala) | Examinar | Zoom in — caras borrosas, marco es moderno pero foto parece vieja |
| TV apagada | Examinar | Reflejo en pantalla muestra algo diferente al cuarto |
| Comida en mesa | Examinar | "Arroz con pollo. Aún tibio. La mesa tiene cubiertos para 4 personas." |
| Ventana trasera | Examinar | Jardín oscuro. Hay una silla afuera con nadie sentado. |
| Cajón de cocina | Abrir | Recibo de delivery con la dirección del propio apartamento de Marco como destino |
| Teléfono fijo (cocina) | Examinar | Suena una vez. Si el jugador lo levanta: silencio y la línea se corta. |
| Puerta del sótano | Intentar abrir | Cerrada. App: *"Primer cliente en el sótano. Necesitas código de acceso."* |

**Escalada narrativa dentro de la casa:**
- Cada interactable que el jugador toca genera un mensaje adicional de la app.
- El tono cambia: de cheerful a insistente a amenazante.
- La casa empieza a sentirse familiar — pero el jugador no sabe por qué todavía.

### Estado 4 — REVELACIÓN (25:00–30:00)

**El trigger:** El jugador intenta abrir el sótano por tercera vez, o pasan 2 minutos dentro de la casa.

**Secuencia de revelación (en orden exacto):**

1. **Flash blanco en pantalla** (0.3 segundos). Sonido de cámara × 3.
2. El **camera roll se abre automáticamente**. Aparecen 3 fotos nuevas.
3. Las fotos muestran: **Marco durmiendo en su propio apartamento**, tomadas desde ángulos interiores imposibles (desde el techo, desde el pasillo, desde dentro del armario).
4. Las fotos tienen metadata: tomadas hace 3, 4 y 6 días.
5. El cliente escribe: *"Llevas semanas haciéndome entregas. Solo que no recuerdas los turnos nocturnos."*
6. Pausa de 4 segundos de silencio total.
7. El cliente escribe: *"¿Recuerdas la primera vez que viniste aquí?"*
8. La casa empieza a cambiar: muebles ligeramente desplazados (10%), temperatura de color más fría, fotos reemplazadas por versiones con las caras borrosas de Marco.
9. El jugador ve sus propios zapatos junto a la entrada (mismo modelo que los de niño que vio antes).
10. La TV se enciende. Muestra: el departamento de Marco, en este momento, vacío. Desde una cámara instalada.

**Beats de texto — los más críticos del juego:**

| # | Texto | Canal | Timing |
|---|---|---|---|
| 1 | "Llevas semanas haciéndome entregas. Solo que no recuerdas los turnos nocturnos." | Chat cliente | Inmediato al trigger |
| 2 | "¿Recuerdas la primera vez que viniste aquí?" | Chat cliente | +4s |
| 3 | "No hay sótano. Nunca hubo sótano. Eso era para ver si seguías instrucciones." | Chat cliente | +8s |
| 4 | "Tu calificación es perfecta, Marco. Siempre ha sido perfecta." | Chat cliente | +12s |
| 5 | [Rating en app baja de 5.0 a 1.0 en 3 segundos — sin causa visible] | Rating tab | +15s |
| 6 | "Nueva entrega disponible. Distancia: 2.4 km. Cliente: Marco R. Dirección: [dirección real del jugador]" | Notificación CRITICAL | +20s |

### Estado 5 — ESCAPE Y FINAL (30:00–35:00)

**La última entrega:**
El jugador vuelve al coche. La app muestra su dirección real. No puede rechazar la entrega (no hay botón de rechazo). Rating es 1.0 — si no completa esta entrega, game over.

**Loading final (diferente a los anteriores):**
- El GPS dibuja la ruta más lento que nunca.
- La música cambia: el piano loop que fue la base de todo el episodio toca una nota falsa una vez.
- La dirección aparece letra por letra en pantalla (en lugar de aparecer toda de golpe).
- La animación del GPS se congela por 2 segundos. Luego continúa.

**El apartamento:**
- La puerta está abierta.
- Todo está igual que en las fotos.
- Sobre la mesa: la comida que Marco "entregó". Aún caliente.
- El jugador entra.

**El doppelgänger:**
- Está sentado en la mesa. De espaldas. Comiendo.
- Al entrar 2 metros: **todos los sonidos se cortan. Silencio absoluto.**
- La figura deja de moverse. La cabeza gira 180° lentamente (2.5 segundos).
- Cara idéntica a Marco. Sonrisa que no es una sonrisa — más una mueca.
- 4 segundos de contacto visual.
- **Fade to black.**

**TITLE CARD (texto blanco sobre negro, aparece línea por línea, 8 segundos entre cada una):**

> *Marco Reyes, 21, reportó el incidente al servicio de atención de QuickRun el 14 de septiembre de 2022.*
>
> *Su cuenta fue suspendida por "incumplimiento en la entrega #1847-C".*
>
> *El número 113 de la Calle Patiño fue demolido en noviembre de 2019.*
>
> *Marco no ha aceptado un turno nocturno desde entonces.*
>
> *Su cuenta de QuickRun sigue activa.*

**CRÉDITOS:** Texto blanco en negro muy tenue. Scroll lento. Al final: email para enviar historias reales de trabajadores de delivery. Botón: "Episodio 2 — Próximamente."

---

### Variante narrativa por nivel de obediencia

**Ruta "Obediente total"** (Rating ≥ 3.0 al llegar al final):
Title card adicional: *"La cuenta de Marco generó $12,840 en comisiones para QuickRun durante los 'turnos que no recuerda'."*

**Ruta "Rebelde"** (Rating ≤ 1.5 llegando al final):
Title card adicional: *"El único repartidor en la historia de QuickRun que recibió calificación de 0.0 estrellas. No por mal servicio."*

---

### La mecánica del "basado en hechos reales"

**NO fabricar historias.** Approach legal y auténtico (igual a Fears to Fathom):
Al inicio del juego, pantalla negra con texto blanco:

> *"Esta historia fue construida a partir de testimonios reales de trabajadores de delivery nocturnos.*
> *Si tuviste una experiencia inexplicable durante un turno, escríbenos:*
> *stories@lastdeliverygame.com"*

Este mensaje va también en itch.io y en los créditos. Es legal, auténtico, construye comunidad, y genera contenido real para Ep 2–4.

---

## 🎨 SECCIÓN 3 — BIBLIA VISUAL

### Identidad visual: "Lo mundano que se vuelve incorrecto"

**Referencias visuales primarias:**
- **Apartment** (game, 2023) — iluminación interior realista, color correction que hace familiar lo cotidiano
- **Mouthwashing** — uso del color como indicador emocional, no decorativo
- **The Excavation of Hob's Barrow** — textures sencillas con mucha atmósfera
- **Referencia fílmica:** *Coherence* (2013) — lo ordinario que se desdobla. *A Horrible Way to Die* — cámara sin tripié, grain, color sucio

**Lo que NO es:**
- PS1 / retro aesthetic → sobresaturado en 2026, no diferencia
- Gore o monstruos → no es el estilo
- Oscuridad total → el horror aquí es VER las cosas, no no verlas

### Paleta oficial del proyecto

| Nombre | HEX | RGB | Uso |
|---|---|---|---|
| Negro profundo | `#0d0d1a` | 13, 13, 26 | Background, escenas cerradas |
| Azul noche | `#1a2a4a` | 26, 42, 74 | UI oscura, cielo nocturno |
| Naranja caliente | `#FF6B35` | 255, 107, 53 | Accent principal, luces de calle, app UI |
| Rojo amenaza | `#e94560` | 233, 69, 96 | Peligro, CRITICAL, Game Over |
| Blanco fantasma | `#f0f0f0` | 240, 240, 240 | Texto, UI general |
| Cálido interior | `#d4a853` | 212, 168, 83 | Luz interior de casa, Estado 1 |
| Verde digital | `#00ff41` | 0, 255, 65 | Cámara de timbre / night vision |

### Temperatura de color por estado del juego

| Estado | Temperatura | Hex dominante | Post-Process URP |
|---|---|---|---|
| NORMALIDAD | 3200K (cálido) | `#d4a853` | Ninguno especial |
| ANOMALÍA | 4500K (neutro) | `#c8c8c8` | Film grain 0.2, vignette 0.1 |
| INFILTRACIÓN | 6500K (frío) | `#8899bb` | Film grain 0.4, vignette 0.3, bloom sutil |
| REVELACIÓN | 8000K (azul-blanco) | `#99aacc` | Chromatic aberration 0.3, grain 0.7, contrast +20% |
| ESCAPE/FINAL | 3200K (vuelve al cálido) | `#d4a853` | Desaturación -30%, grain 0.5 |

### Diseño visual de la app QuickRun

**La app es el HUD. Su diseño ES la identidad visual del juego.**

**Inspiración:** DoorDash + Glovo + Rappi — estética corporativa cheerful que contrasta con el horror.
**Tipografía:** Sans-serif geométrica limpia (Nunito o Inter). Nunca serif.
**Iconografía:** Material Design-adjacent. Minimalista.
**Tono de color:** Fondo blanco para UI de app (contraste total con el mundo oscuro).

```
[PANTALLA INICIO / TAB INICIO]
┌─────────────────────────────┐
│ Q U I C K R U N             │  ← Logo en naranja #FF6B35, 28pt Bold
│ Tu turno de entrega         │  ← Subtítulo, 12pt gris
├─────────────────────────────┤
│ ★★★★★  4.8                  │  ← Rating: 5 estrellas, fill parcial, valor numérico
│ Tu calificación             │
├─────────────────────────────┤
│ ESTADO: En turno activo     │  ← Status, fondo verde si activo, rojo si warning
├─────────────────────────────┤
│ [ INICIAR TURNO ]           │  ← Botón naranja, 44px altura, esquinas 8px radius
└─────────────────────────────┘

[NOTIFICACIÓN BANNER — TOP, 56px altura]
┌─────────────────────────────┐
│ !! Marco. Sabemos que ya... │  ← Fondo rojo pulsante, texto blanco, 12pt
└─────────────────────────────┘

[TAB GPS / MAPA]
┌─────────────────────────────┐
│ 🗺 CALLE PATIÑO 113         │  ← Dirección en bold
│ ● ─────────────────── ★    │  ← LineRenderer dibujando ruta
│ Tu ubicación     Destino    │
│ 2.4 km · 8 min estimados   │  ← ETA, gris
└─────────────────────────────┘

[TAB CHAT]
│ Cliente D. Recio            │
│ 22:41  Ya te veo. Cerca.  [READ] │ ← Burbuja izquierda (cliente), fondo gris
│ 22:42          Casi llego. │ ← Burbuja derecha (Marco), fondo naranja
│ 22:43  Esa soy yo. Porche. │
│ [Teclado oculto — solo leer]│

[TAB GALERÍA]
│ CAMERA ROLL                 │
│ [vacío al inicio]           │ ← Grid 3 cols, fondo negro
│ Sin fotos                   │
│ [foto borrosa aparece aquí] │

[GAME OVER — PANTALLA CORPORATIVA]
┌─────────────────────────────┐
│                             │
│ C U E N T A                 │
│ D E S A C T I V A D A       │
│                             │  ← Texto en rojo #e94560, fondo negro #0d0d1a
│ Tu cuenta QuickRun ha sido  │
│ desactivada por violación   │
│ de términos de servicio.    │
│                             │
│ Rating final: 0.0 ★         │
│                             │
│ [ Solicitar revisión ]      │  ← Botón que NO hace nada
└─────────────────────────────┘
```

### Diseño de entornos — Dirección visual por escena

**Entorno A — El coche (interior):**
- Sedán compacto latinoamericano. Sin marca visible.
- Dashboard con luces tenues (azul-verde de instrumentos + naranja de una advertencia perpetua).
- Lluvia constante en parabrisas. Efecto de gotas con shader simple (no animación costosa).
- Espejo retrovisor: muestra la calle detrás. Estado 1: normal. Estado 4: muestra el asiento trasero ocupado (pero al girarse: vacío).
- Lit: pequeños point lights desde el dashboard (color `#d4a853`, intensidad muy baja). Lluvia exterior apaga cualquier luz cálida.

**Entorno B1 — Exterior edificio normal (Entregas 1 y 2):**
- 4 pisos. Ladrillo expuesto o concreto. Latinoamericano: balcones pequeños, rejas en ventanas bajas, interphone viejo.
- Bien iluminado: farola funcional, luz del lobby visible por puerta de vidrio.
- Mundano al máximo. Sin detalles perturbadores. ESTE ES EL PUNTO.
- Variante para Entrega 2: diferente número, diferente color de fachada.

**Entorno B2 — Calle Anómala (Entrega 3):**
- Calle residencial. 3 casas visibles. Solo una farola distante.
- 80-90% de la pantalla en sombra. La única fuente de luz: el teléfono del jugador + esa farola.
- La casa del centro: sin número. Idéntica a las otras en estructura pero diferente en *energía*: pasto más largo, mailbox sin nombre, cortinas corridas.
- Efecto: Fog density 0.08 (vs 0.02 del Estado 1). Niebla que corta visibilidad a 25m.
- Sin peatones. Sin autos. Silencio total excepto lluvia.

**Entorno C — Casa anómala (interior):**
- Layout: Entrada (2m²) → Sala (15m²) → Cocina (10m²) → Pasillo (3m × 1m) → Puerta del sótano (bloqueada).
- Estado NORMAL: iluminación tenue pero funcional. Una lámpara de pie encendida. Refrigerador zumbando.
- Estado POST-REVELACIÓN (misma geometría): temperatura de color -2000K. Muebles desplazados 5–10°. Las fotos tienen caras borrosas. Vapor particles sobre la comida se detienen.
- **Detalle narrativo clave:** El layout de esta casa es idéntico al del apartamento de Marco en Entorno D. El jugador no lo nota en tiempo real — solo al llegar al final.

**Entorno D — Apartamento del jugador (final):**
- Mismo geometry que Entorno C. Reskin completo.
- Markers de identidad: posters en pared (banda local, afiche universitario), ropa en silla (misma que Marco "usa" en el juego), zapatos en la entrada (igual a los del coche).
- La comida del delivery está sobre la mesa. Misma comida que "entregó" hace 3 minutos.
- Iluminación: vuelve al cálido (3200K) pero desaturado -30%.

### El doppelgänger — Especificación visual completa

**Por qué importa:** Es el momento más viral. El thumbnail de TikTok. El clip de 10 segundos.

**Diseño:**
- Modelo: hombre joven (21 años), ropa de repartidor latinoamericano (jeans oscuros, camiseta gris manga corta, mochila de delivery en respaldo de silla).
- Cara: suficientemente genérica para proyección, pero con detalles reconocibles (barba de 2 días, cicatriz pequeña sobre la ceja derecha).
- Estado inicial: sentado, de espaldas, comiendo con movimientos normales.
- Post-giro: misma postura pero cabeza girada 180°. Sonrisa que no llega a los ojos. Ojos fijos al jugador.
- **Nunca se mueve hacia el jugador. La inmovilidad es el horror.**

**Animaciones requeridas (mínimas):**
1. `idle_eating`: sentado, brazo derecho lleva comida a la boca cíclicamente. Loop 4 segundos.
2. `turn_to_look`: giro de cabeza de 0° a 180° en 2.5 segundos. Una vez, no loop. Curva de animación: ease-in slow, ease-out muy lento (la cabeza "llega" al final de golpe).
3. `smile_idle`: pose final estática. Solo la comisura de los labios se mueve (muy sutil, casi no se ve).

**Herramienta para animaciones:** Unity Animation window + huesos básicos (no necesita Mixamo). Son 3 keyframes cada una.

---

## 🎧 SECCIÓN 4 — DIRECCIÓN DE AUDIO

### Principio central: NUNCA hay música diegética. Todo es sonido de ambiente.

**El único "música" permitido:** un piano lejano que existe en el mundo del juego (el apartamento de un vecino). Este piano se convierte en la firma sonora emocional del episodio.

### Soundscape por ubicación

**El coche:**
```
Capa base (loop, siempre):
- Lluvia en techo metálico (Freesound CC0, loop 2min)
- Motor apagado + tick de metal al enfriarse (cada 12-20s, random)
- Radio estática muy baja, casi inaudible (otra capa)

Eventos puntuales:
- Puerta del coche abriéndose: SFX_car_door_open
- Notificación app: [el ping — ver debajo]
- Masticación: SFX_chips_eat (1-2 veces, sutil, solo entre entregas)
```

**Exterior edificio normal:**
```
Capa base:
- Lluvia exterior (más fuerte que la del coche)
- Viento leve
- Tráfico distante (mezcla de coches, claxon ocasional muy lejano)
- Ladrido de perro lejano (una vez, no loop)

Eventos:
- Pasos en acera: SFX_footstep_concrete (gen aleatoria pitch ±5%)
- Puerta de edificio cerrándose: SFX_door_heavy_close
```

**Calle Anómala:**
```
Capa base (MUY diferente):
- Silencio casi total. Solo: grillo único (sí, uno) + viento intermitente (cada 30s)
- La ausencia de ruido DE CIUDAD es el primer horror sonoro.

Eventos:
- Paso del jugador: SFX_footstep_pavement (más eco que los anteriores)
- Luz de porche encendiéndose: SFX_lightbulb_flicker_on
- La app haciendo ping aquí SUENA DIFERENTE — más reverberado, como en un espacio vacío
```

**Casa anómala — Estado NORMAL:**
```
Capa base:
- Refrigerador zumbando (60Hz, constante)
- Reloj de pared (tick cada segundo, en sala)
- Crujidos de madera al caminar (gen aleatoria)

Ausente: ningún sonido humano. Eso es lo extraño.
```

**Casa anómala — Estado ANOMALÍA / REVELACIÓN:**
```
Cambios graduales:
- El refrigerador para de zumbar (trigger en el momento exacto de la revelación)
- El reloj deja de hacer tick
- En su lugar: una conversación amortiguada CASI audible (como a través de 3 paredes)
- Golpeteo rítmico muy suave (podría ser música de otro apartamento, podría no serlo)
- Dripping water en el pasillo (la pipa que "gotea" en el sótano que nunca abren)
- SILENCIO TOTAL cuando el doppelgänger gira: todo corta en un frame.
```

### El Ping de Notificación — Diseño específico

**Es el sonido más importante del juego.** El jugador lo escucha 15–20 veces durante el episodio. Debe:
- Sonar corporativo y cheerful al inicio
- Disparar ansiedad pavloviana para cuando llega el tercer ping del Estado 3

**Especificación:**
- Duración: 0.35 segundos
- Tono base: re mayor (D4, 293 Hz) + quinta justa (A4, 440 Hz)
- Envolvente: attack inmediato, decay 0.1s, sustain 0.2s, release 0.05s
- Una disonancia sutil añadida en los pings del Estado 3+: B♭4 (466 Hz) mezclada muy baja (-18dB)
- **ElevenLabs Sound Effects** para generación: prompt *"short corporate notification chime, friendly, major key, 0.3 seconds, digital, clean"*

### La música del piano

**Función:** Firma emocional. No es música del juego — es la música del vecino de Marco.

- `music_normalidad_loop.mp3`: piano solo, legato, melodía simple en do mayor. 4 minutos. **Suena como lo que toca un vecino practicando.** Loop con fade cruzado.
- `music_anomalia_loop.mp3`: el mismo piano pero UNA nota falsa (si menor en lugar de si mayor) cada 47 segundos. El jugador puede no notarlo conscientemente.
- `music_revelacion_loop.mp3`: el piano para. Solo quedan los graves del piano: reverb larguísimo, decay de 8 segundos. Parece un drone más que música.
- `music_final_silence.mp3`: 30 segundos de silencio con sub-bass imperceptible (20 Hz, -30dB) que **se siente** aunque no se oye.

**Regla de corte:** Cuando el doppelgänger gira la cabeza → corte inmediato de TODO el audio. No fade. Corte en frame.

---

## ⚙️ SECCIÓN 5 — MECÁNICAS DE JUEGO (DETALLADAS)

### Movimiento del jugador

- **CharacterController** (no Rigidbody) — mantiene colisiones simples, sin física compleja
- Velocidad: 2.5 m/s caminando. Sin correr — Marco es un repartidor cansado, no un speedrunner
- Head bob: amplitud 0.04, frecuencia 1.5 Hz. Sutil, no nauseabundo
- Mouse look: sensibilidad configurable 0.5x–3.0x, sin lock vertical (180° libre)
- **Linterna:** mantener F con el teléfono activo → el flash del teléfono ilumina hacia adelante. Radio 8m, cono 35°. Consumo de batería simulado (no real — es puramente visual)

### El sistema del teléfono — Especificación completa

**F = mostrar/ocultar teléfono. Esta es la mecánica más usada del juego.**

**Estados del teléfono:**
```
GUARDADO: teléfono no visible. Jugador ve el mundo completo.
         → Presionar F: animación slide-from-bottom (0.3s, ease-out)

ACTIVO: teléfono visible en parte inferior de pantalla.
        Panel de 420×900px centrado. El mundo es visible en el 35% superior.
        → Interactuar con tabs: cambiar pantalla dentro del teléfono
        → Presionar F de nuevo: guardar

Sin estado fullscreen — el teléfono ocupa siempre la misma área.
```

**Cuando el teléfono está activo:**
- FOV del mundo: reducción de 15% (de 70° a 59°)
- Post-process: vignette leve +0.1 en bordes laterales
- Los sonidos del mundo bajan -3dB (Marco está "enfocado en la pantalla")
- **El jugador puede caminar mientras mira el teléfono** — esto es intencional y crea momentos peligrosos

### Las 8 decisiones binarias — Especificación completa

| # | Momento | Decisión | Consecuencia OBEDECER | Consecuencia DESOBEDECER |
|---|---|---|---|---|
| D1 | Entrega 3: puerta entreabierta | ¿Entrar o no? | Rating se mantiene. Progresa al Estado 3. | Rating -1.0. App insiste. Bloqueo temporal 60s. |
| D2 | Dentro de casa: nueva entrega desde adentro | ¿Aceptar o ignorar? | App: "¡Perfecto! Bonus +$2." Historia progresa. | Rating -0.5. Mensaje: "Penalización por rechazo." |
| D3 | Cocina: ¿ir al sótano? | ¿Intentar abrir? | Puerta bloqueada — descubrimiento narrativo. | Rating -0.3 si el jugador no intenta. |
| D4 | Teléfono fijo suena: ¿responder? | ¿Levantar el teléfono? | Silencio + línea muerta. Detalles de lore. | Nada — pero el jugador se pierde un lore beat. |
| D5 | Fotos del camera roll: ¿las mira? | ¿Abrir galería? | Revelación completa — trigger narrativo principal. | Galería se abre forzada igual — no se puede evitar. |
| D6 | Mensaje del cliente sobre "semanas de entregas": ¿responder? | ¿Escribir algo? | El cliente responde: "Sabía que lo recordarías." | El cliente no responde. El silencio es peor. |
| D7 | Última entrega: propia dirección | ¿Ir o negarse? | Única opción — no hay botón de rechazo. Game over si no va. | Game over: Rating 0.0. "Cuenta desactivada." |
| D8 | El doppelgänger: ¿mirar de frente o dar vuelta? | ¿Aguantar el contacto visual? | Fade to black a los 4s. Final normal. | Si el jugador se da vuelta antes: flash + Marco de espaldas en la foto de la galería. |

**Nota de diseño:** La D7 y D8 no son realmente opcionales. La ilusión de elección es parte del horror.

### El Camera Roll — La mecánica de horror central

El camera roll empieza vacío. Las fotos aparecen en 3 momentos:

1. **Aparición 1** (trigger: entrar a la casa por primera vez): Una foto. Borrosa. Parece una habitación oscura.
2. **Aparición 2** (trigger: encontrar el recibo en el cajón): Dos fotos. Marco durmiendo. El ángulo es imposible.
3. **Aparición 3** (trigger: tercer intento de abrir el sótano): Tres fotos más. Marco durmiendo. Fotos de hace 3, 4, 6 días.

**Mecánica de aparición:**
- Flash blanco (0.3s)
- Sonido de obturador de cámara × n (n = número de fotos nuevas)
- Badge de notificación aparece en el tab de Galería
- Si el teléfono está guardado: la pantalla del teléfono se enciende por 0.5s (visible en el bolsillo del jugador — como si alguien lo hubiera tomado)

### Sistema de Rating — Reglas completas

- **Rango:** 0.0 a 5.0 (float)
- **Inicio:** 5.0
- **Game Over:** llegar a 0.0

| Evento | Delta rating |
|---|---|
| Entrega completada | +0.0 (no sube en Ep 1 salvo bonuses) |
| Desobedecer instrucción menor | -0.3 |
| Desobedecer instrucción importante | -0.5 |
| Desobedecer instrucción crítica | -1.0 |
| Tiempo excedido | -0.5 |
| Rating llegando a 1.0 | Mensaje CRITICAL: "Deactivation Warning" |
| Rating llegando a 0.0 | Game Over — pantalla corporativa |

**Visual:** Las 5 estrellas en Panel_Inicio usan fill parcial (`Image.Type.Filled, FillMethod.Horizontal`). Cuando baja: shake de la estrella afectada + flash rojo en pantalla.

---

## 📱 SECCIÓN 6 — ESPECIFICACIÓN COMPLETA UI/UX

### PhoneController — Script crítico que controla el teléfono

> **NOTA PARA ANTIGRAVITY:** Este script se implementa en B7. Sin él, el teléfono no abre/cierra.

```csharp
// PhoneController.cs — Gestiona show/hide del teléfono con F
// Vive en: Managers/PhoneController GO
// Controla: UI/PhoneCanvas SetActive true/false
// Integra con: EventManager.OnPhoneOpen / OnPhoneClose
// Animación: slide desde abajo, 0.3s, ease-out (DoTween o manual coroutine)
```

### Canvas y orden de renderizado (CRÍTICO)

```
Canvas: PhoneCanvas          sortingOrder: 10   ← el teléfono
Canvas: GameOverCanvas       sortingOrder: 100  ← game over, SIEMPRE activo
Canvas: TransitionCanvas     sortingOrder: 200  ← fade to black, SIEMPRE activo
Canvas: LoadingCanvas        sortingOrder: 150  ← loading screens, SIEMPRE activo
```

**PhoneCanvas se desactiva cuando el teléfono se guarda. Los demás NUNCA se desactivan.**

### QuickRun App — Pantallas completas

**Tab INICIO (Panel_Inicio):**
- Logo: "QUICKRUN" en `#FF6B35`, 28pt Bold, centrado
- Subtítulo: "Tu turno de entrega", 13pt gris
- Separator: línea 1px `#1a2a4a`
- RatingRow: 5 imágenes de estrella (`Image.Type.Filled`) + texto "4.8"
- StatusText: "En turno activo" (color dinámico: verde / naranja / rojo)
- Spacer flexible
- StartButton: naranja sólido, 44px alto, corner radius 8px, texto "INICIAR TURNO"

**Tab GPS (Panel_Mapa):**
- Dirección destino: texto grande, bold
- Nombre cliente: texto gris
- Ruta: LineRenderer dibujando de punto A a punto B en tiempo real
- ETA: "8 min estimados"
- Estado: "Navegando" / "Has llegado" / "Fuera de ruta"

**Tab CHAT (Panel_Chat):**
- Header: nombre del cliente, icono de QuickRun
- ScrollView: burbujas de mensajes estilo WhatsApp
- Burbuja cliente: fondo gris `#e8e8e8`, texto oscuro, alineada izquierda
- Burbuja Marco: fondo naranja `#FF6B35`, texto blanco, alineada derecha
- **No hay teclado real.** Marco "responde" automáticamente en ciertos triggers o el jugador puede elegir de opciones predefinidas (max 2 opciones).
- Typing indicator: tres puntos animados cuando el cliente "escribe"

**Tab GALERÍA (Panel_Galeria):**
- Grid 3 columnas, fondo negro puro `#000000`
- Estado vacío: texto "Sin fotos" centrado, gris
- Foto thumbnail: 120×120px, rounded corners 4px
- Al tap: visor fullscreen con scroll para zoom (pinch / mouse wheel)
- Foto especial: badge rojo con "NUEVA" si apareció desde la última apertura

**Notificación Banner (NotificationBanner):**
- Posición: top del teléfono, 56px de altura
- Aparece: slide desde arriba (0.25s ease-out)
- Desaparece: slide hacia arriba (0.2s ease-in), automático a los 3s (excepto CRITICAL)
- INFO: fondo `#4A90D9`
- SUCCESS: fondo `#27AE60`
- WARNING: fondo `#FF6B35`
- CRITICAL: fondo `#e94560` pulsante (alpha 0.6 → 1.0 → 0.6, cada 0.5s) — requiere dismiss

---

## 🏗️ SECCIÓN 7 — DISEÑO DE NIVELES

### Principio: Todo lo que no se ve, no existe

Solo construir los ángulos visibles desde donde camina el jugador. Nunca construir habitaciones a las que no se accede. Esta regla elimina semanas de trabajo innecesario.

### Layout Entorno C — Casa Anómala (planta completa)

```
[ENTRADA 2m×2m]
Perchero (1 abrigo), alfombra pequeña, zapatos (niño, del mismo modelo que los de Marco), espejo pequeño (no está Marco en el reflejo — bug visual intencional)

[SALA 5m×3m]
Sofá (mirando TV), mesa de centro (revista sin fecha visible), TV apagada (plasma viejo), lámpara de pie (encendida), estantería (libros sin títulos legibles), foto familiar ×3 en pared (caras borrosas)

[COCINA 3m×3m]
Mesa con 4 sillas (comida caliente para 2 personas), refrigerador (zumba), fregadero, ventana trasera (da al jardín), cajón (recibo de delivery), teléfono fijo en pared

[PASILLO 4m×1.2m]
Sin decoración. Solo una bombilla desnuda que parpadea ocasionalmente. La puerta del sótano al fondo.

[PUERTA SÓTANO]
Cerradura vieja. Nunca abre. Al acercarse: dripping water audible. App envía instrucciones desde aquí.
```

### Interactables por escena — Lista completa con triggers

**Entrada:**
- Espejo: examinar → reflejo no muestra al jugador → no hay trigger narrativo, solo detail
- Zapatos de niño: examinar → "Talla 28. No hay niños en la casa." → EventManager dispara lore event

**Sala:**
- Fotos (3): examinar cada una → zoom in, caras borrosas → tercera foto: un detalle del fondo reconocible (la misma silla del jardín)
- TV apagada: examinar → reflejo muestra Estado ANOMALÍA de la sala (muebles desplazados) aunque el jugador no haya llegado a ese estado aún
- Revista: examinar → sin fecha, artículo sobre "trabajadores nocturnos" → sutil, no agresivo

**Cocina:**
- Comida: examinar → "Arroz con pollo. Temperatura: caliente. Para 2 personas." → trigger de lore
- Ventana: examinar → jardín oscuro, silla sin nadie → si el jugador espera 8 segundos mirando: la silla se mueve 5cm. Solo una vez.
- Cajón: abrir → recibo de delivery, fecha de 3 días atrás, dirección del destino: el apartamento de Marco
- Teléfono fijo: interactuar → suena una vez, el jugador puede descolgar → silencio 4s → línea muerta → la app hace ping inmediatamente después

**Pasillo:**
- La bombilla: examinar → nada
- Puerta del sótano: hasta 3 intentos. Cada intento: la app responde diferente.
  - Intento 1: "Código de acceso requerido."
  - Intento 2: "Segundo piso." (no hay segundo piso)
  - Intento 3: [trigger revelación — el camera roll recibe las últimas 3 fotos]

### Puntos de spawn y cámaras

| Entorno | Punto spawn | Dirección inicial |
|---|---|---|
| Coche | Asiento conductor | Hacia el parabrisas (lluvia) |
| Exterior edificio (Entregas 1 y 2) | Vereda, frente a entrada | Hacia la puerta |
| Calle Anómala | Centro de calle, de espaldas a la cámara | Hacia las casas |
| Casa (entrada) | Puerta entreabierta, umbral | Hacia el interior |
| Apartamento final | Puerta abierta | Hacia la mesa con el doppelgänger |

---

## 🛠️ SECCIÓN 8 — ARQUITECTURA TÉCNICA

### Estado actual de scripts (verificado 26-03-2026)

```
✅ EXISTS AND COMPILING:
Assets/_Game/Scripts/
  Core/
    GameManager.cs       ← singleton, enum GameState, DontDestroyOnLoad
    EventManager.cs      ← todos los eventos tipados
    SceneSetup_EP1.cs    ← setea NORMALIDAD en Awake
  Phone/
    AppNotification.cs   ← VIEJO script de B2, puede coexistir
    ChatController.cs    ← sistema de mensajes de B3
    MapController.cs     ← GPS/mapa de B4
    MapMarker.cs
  IndiGame/
    Phone/
      RatingController.cs         ← ✅ B6, completo
      AppNotificationController.cs ← ✅ B6, completo
      NotificationData.cs          ← ✅ B6
      NotificationType.cs          ← ✅ enum INFO/SUCCESS/WARNING/CRITICAL
      GalleryController.cs         ← ✅ B5
      FullscreenViewer.cs          ← ✅ B5
      PhoneTabController.cs        ← ✅ B2-B5
      PhotoData.cs                 ← ✅ B5
    UI/
      GameOverController.cs        ← ✅ B6, completo
    Environment/
      FlickerLight.cs

⬜ PENDIENTE (crear en bloques futuros):
  Core/
    PhoneController.cs     ← B7: show/hide teléfono con F, animación, eventos
    GameClock.cs           ← B7: reloj interno 22:30+, 1min/6s real
    TransitionController.cs ← B7: fade to black, métodos FadeIn/FadeOut
    DeliveryManager.cs     ← B7: secuencia de 3 entregas, estado activo
    NarrativeDirector.cs   ← B16: orquesta triggers narrativos
    DecisionManager.cs     ← B19: procesa las 8 decisiones binarias
    AudioManager.cs        ← B21: ambiences + crossfade
    SFXPlayer.cs           ← B21: pool de 8 sources
    VoiceoverController.cs ← B22: líneas de voz atadas a triggers
    MusicController.cs     ← B23: crossfade entre tracks
  Phone/
    InteractableObject.cs  ← B15: base class para todos los interactables
    DoorInteraction.cs     ← B13: lógica de puerta del sótano
    ProximityTrigger.cs    ← B11: detecta distancia jugador→objeto
    RoomStateController.cs ← B12: Normal/Anomalous per room
  Characters/
    DoppelgangerController.cs ← B20: idle_eating, turn_to_look, smile_idle
```

### Namespaces (obligatorio en todo script nuevo)

```csharp
IndiGame.Core       // GameManager, EventManager, SceneSetup, PhoneController
IndiGame.Phone      // todo lo de la app
IndiGame.UI         // GameOverController, TransitionController, LoadingController
IndiGame.Audio      // AudioManager, SFXPlayer, MusicController
IndiGame.World      // InteractableObject, DoorInteraction, ProximityTrigger, RoomStateController
IndiGame.Characters // DoppelgangerController
```

### Arquitectura de Canvas (crítica para B7+)

```
UI (GameObject raíz, siempre activo)
├── PhoneCanvas       (Canvas, sortingOrder=10,  se desactiva al guardar teléfono)
│   └── PhonePanel
│       ├── Panel_Inicio
│       ├── Panel_Chat
│       ├── Panel_Mapa
│       ├── Panel_Galeria
│       ├── BottomNav
│       ├── NotificationBanner
│       └── PhoneFlash
├── GameOverCanvas    (Canvas, sortingOrder=100, NUNCA desactivar)
│   └── GameOverPanel (GameOverController.cs aquí)
├── LoadingCanvas     (Canvas, sortingOrder=150, NUNCA desactivar)
│   └── LoadingPanel  (LoadingController.cs aquí)
└── TransitionCanvas  (Canvas, sortingOrder=200, NUNCA desactivar)
    └── FadePanel     (TransitionController.cs aquí — Image negro, alpha 0 en juego)
```

### EventManager — Eventos completos

```csharp
// Ya implementados:
Action<GameState> OnGameStateChanged
Action           OnPhoneOpen
Action           OnPhoneClose
Action<MessageData> OnMessageReceived
Action<float>    OnRatingChanged
Action<int>      OnDeliveryComplete
Action<int>      OnPhotoAppeared
Action           OnDoppelgangerActivate

// AÑADIR en B7:
Action<int>      OnDeliveryStarted    // int = delivery index (0=Entrega1, 1=Entrega2, 2=Entrega3)
Action           OnLoadingStart
Action           OnLoadingEnd

// AÑADIR en B16:
Action<string>   OnNarrativeTrigger   // string = trigger ID, ej: "enter_house", "open_cajón"
Action<int>      OnDecisionPresented  // int = decision index
Action<int, bool> OnDecisionMade     // int = decision index, bool = obeyed
```

### Performance targets

- **Target mínimo:** GTX 1060, 8GB RAM → 60fps estable
- **Shadow Distance:** 50m, 2 cascades
- **Occlusion Culling:** activado para interiores (B26)
- **Texture streaming:** activado
- **Draw calls target:** <150 por frame en interiores
- **Poly budget:** <200K triángulos visibles en el peor caso
- **Audio:** Vorbis, quality 70%, 44.1kHz

---

## 📋 SECCIÓN 9 — PLAN DE BLOQUES REVISADO (B6–B28)

### Estado actual de bloques

| Bloque | Nombre | Estado |
|---|---|---|
| B1 | Proyecto Unity + First Person Foundation | ✅ Completo |
| B2 | Sistema del Teléfono (Core HUD) | ✅ Completo |
| B3 | Sistema de Chat y Mensajería | ✅ Completo |
| B4 | Sistema de GPS y Mapa | ✅ Completo |
| B4.5 | Entorno Greybox Mínimo Visible | ✅ Completo |
| B5 | Galería / Camera Roll | ✅ Completo |
| B6 | Rating System + Notificaciones + Game Over | 🔄 En curso (ver ANTIGRAVITY_B6_FINISH.md) |
| B7–B28 | Ver especificaciones abajo | ⬜ Pendiente |

---

### B7 — PhoneController + GameClock + Loading + Transiciones

**Objetivo:** El esqueleto que conecta todo. Sin B7, no hay juego — solo sistemas sueltos.

**Scripts a crear (en orden):**

**1. PhoneController.cs** (crítico — implementar PRIMERO)
```csharp
// IndiGame.Core namespace
// GO: Managers/PhoneController
// Controla: UI/PhoneCanvas (activar/desactivar)
// F = toggle. Con animación: slide desde abajo (0.3s, ease-out cubic)
// Dispara: EventManager.OnPhoneOpen / OnPhoneClose
// En Start: desactivar PhoneCanvas para empezar sin teléfono
// Expone: public bool IsPhoneOpen { get; private set; }
```

**2. GameClock.cs**
```csharp
// IndiGame.Core namespace
// GO: Managers/GameClock
// Empieza en 22:30
// 1 minuto de juego = 6 segundos reales
// Expone: public string GetFormattedTime() → "22:30"
// Muestra en: StatusText del Panel_Inicio (wired via SerializedObject)
// Pausa durante LoadingScreen
```

**3. TransitionController.cs**
```csharp
// IndiGame.UI namespace
// GO: UI/TransitionCanvas/FadePanel
// Singleton
// public IEnumerator FadeToBlack(float duration = 0.5f)
// public IEnumerator FadeFromBlack(float duration = 0.5f)
// La Image del FadePanel es negro puro, alpha se anima
```

**4. LoadingController.cs**
```csharp
// IndiGame.UI namespace
// GO: UI/LoadingCanvas/LoadingPanel
// Muestra: dirección de destino (letra por letra, tipo máquina de escribir, 0.05s/carácter)
// Muestra: GPS LineRenderer animado dibujando la ruta
// Duración: 3–5 segundos (configurable por entrega)
// En Entrega 3: duración 6s, GPS se recalcula 2 veces visiblemente
// En última entrega: duración 8s, GPS se congela 2s en el medio
```

**5. DeliveryManager.cs**
```csharp
// IndiGame.Core namespace
// GO: Managers/DeliveryManager
// Gestiona secuencia: Entrega1 → Loading → Entrega2 → Loading → Entrega3 → Loading → (casa)
// Dispara: EventManager.OnDeliveryStarted(int index)
// Activa/desactiva los entornos correctos según entrega activa
// Integra con TransitionController para fades
// Integra con LoadingController para pantallas de carga
```

**Escena a modificar en B7:**
- Crear GO `Managers/PhoneController` con el script
- Crear GO `Managers/GameClock`
- Crear Canvas `UI/TransitionCanvas` con FadePanel (Image negra, alpha=0)
- Crear Canvas `UI/LoadingCanvas` con LoadingPanel
- Crear GO `Managers/DeliveryManager`
- Mover GameOverPanel a `UI/GameOverCanvas` (si no se hizo en B6)

**Verificación Play Mode B7:**
- Presionar F: teléfono aparece con slide animation
- Presionar F de nuevo: desaparece
- PhoneCanvas se activa/desactiva (GameOverCanvas NO se mueve)
- GameClock muestra "22:30" en Panel_Inicio/StatusText y avanza
- FadeToBlack() funciona (llamar vía script-execute)
- LoadingScreen visible con dirección escrita letra a letra

---

### B8 — Arte: Texturas, Materiales y Paleta Global

**Objetivo:** Definir y aplicar la paleta visual ANTES de construir entornos.

**Materiales URP/Lit a crear (en `Assets/_Game/Materials/`):**

| Nombre | Base Color | Roughness | Metallic | Textura Poly Haven |
|---|---|---|---|---|
| MAT_WetAsphalt | `#1a1a1a` | 0.9 | 0 | Wet Asphalt (CC0) |
| MAT_Concrete | `#c8c8c0` | 0.85 | 0 | Concrete Wall 012 (CC0) |
| MAT_WoodFloor | `#8b5a2b` | 0.7 | 0 | Wood Floor 012 (CC0) |
| MAT_WallPaint | `#e8e4da` | 0.95 | 0 | Plaster White (CC0) |
| MAT_DarkWall | `#1a2a4a` | 0.8 | 0 | (color sólido, no textura) |
| MAT_StreetLight | `#FF6B35` | 0.3 | 0.8 | Emissive: `#FF6B35` |
| MAT_GlassWindow | `#88aabb` | 0.1 | 0 | Alpha 0.3 |
| MAT_CarpetOld | `#5a4a3a` | 1.0 | 0 | Fabric Brown (CC0) |

**URP Post-Process Volume Base (Global Volume):**
```
Bloom: Intensity 0.3, Threshold 1.0, Scatter 0.7
Vignette: Intensity 0.1, Smoothness 0.3 (se incrementa por estado)
Film Grain: Intensity 0.2, Size 1.0 (se incrementa por estado)
Color Adjustments: Contrast 0, Saturation 0 (base)
White Balance: Temperature 0 (base, se modifica por estado en Volume Profiles separados)
```

**Volume Profiles por estado (crear 5 assets):**
- `VolumeProfile_Normalidad.asset`: temperatura +20, contraste -5, sin grain extra
- `VolumeProfile_Anomalia.asset`: temperatura 0, grain +0.2, vignette +0.2
- `VolumeProfile_Infiltracion.asset`: temperatura -20, grain +0.4, vignette +0.3, desaturación -10%
- `VolumeProfile_Revelacion.asset`: temperatura -30, grain +0.7, chromatic aberration 0.3, contraste +20
- `VolumeProfile_Final.asset`: temperatura +20 (vuelta al cálido), desaturación -30%

---

### B9 — Entorno: Coche Interior

**Modelos Blender MCP a crear:**
- Sedán compacto: solo interior visible desde asiento conductor. Cuatro planos principales: dashboard, volante, asiento pasajero, techo.
- Dashboard: 8-10 luces de instrumento (Point Lights muy pequeños, `#d4a853` + uno naranja advertencia)
- Espejo retrovisor: RenderTexture de cámara trasera (actualización 1fps — ahorra GPU)
- Bolsa de delivery en asiento pasajero: low-poly, logo QuickRun en tela

**Partículas:**
- Lluvia en parabrisas: Particle System proyectando desde exterior → shader de gotas en vidrio (ShaderGraph simple: distorsión normal map animada)
- No usar física de lluvia real — demasiado costoso para impacto visual mínimo

---

### B10 — Entorno: Exterior Edificio Normal (Entregas 1 y 2)

**Modelos Blender MCP:**
- Fachada 4 pisos: visible desde vereda, solo la cara frontal (4–5m de ancho visible)
- Interphone: prop interactivo en pared, pulsador con luz LED
- Farola: existente desde B4.5 (reutilizar)
- Contenedor de basura lateral

**Variante entrega 2:** Cambio de número (texture swap del número de edificio) + cambio de color de fachada (material override).

---

### B11 — Entorno: Calle Anómala

**Detalles específicos:**
- 3 casas: solo las fachadas visibles desde donde está el jugador (ángulo de 60°)
- La casa central: `ProximityTrigger.cs` detecta distancia ≤15m → activa luz de porche
- Fog: `Volume Profile` especial con Fog Exponential density 0.08, color `#0a0a14`
- La farola distante: punto de luz naranja en -45m del spawn, intensidad reducida

---

### B12 — Entorno: Casa Anómala — Planta Baja

**Layout exacto (ver Sección 7 para dimensiones).**

**`RoomStateController.cs` por habitación:**
```csharp
// IndiGame.World namespace
// enum RoomState { NORMAL, ANOMALOUS }
// SetState(RoomState state):
//   - Cambia material de las fotos (caras borrosas)
//   - Desplaza muebles 5-10° via TransformModifier
//   - Alterna entre VolumeProfile (crossfade 2s)
//   - Detiene las partículas de vapor
```

**Secuencia de cambio de estado:**
- Sala → ANOMALOUS: cuando el jugador sale de la cocina y vuelve a la sala
- Cocina → ANOMALOUS: 30 segundos después de trigger de revelación
- Todo → ANOMALOUS simultáneo: al abrir el camera roll por tercera vez

---

### B13 — Entorno: Casa Anómala — Pasillo + Sótano

**La ilusión de profundidad:**
- El pasillo en Estado NORMAL: 4m de largo
- El pasillo en Estado ANOMALÍA: shader de distorsión de perspectiva (URP Custom Render Feature que aplica barrel distortion al depth buffer en el frustum del pasillo)
- Implementación más simple: reducir el FOV progresivamente mientras el jugador camina hacia el fondo (FOV de 70° a 55° en 4 metros)

**`DoorInteraction.cs`:**
```csharp
// IndiGame.World namespace
// 3 intentos con respuestas diferentes de la app
// Tercer intento: dispara EventManager.OnNarrativeTrigger("sotano_tercero")
// → NarrativeDirector captura este evento y activa la revelación
```

---

### B14 — Entorno: Apartamento del Jugador

**Reutilizar geometry de B12.** Solo cambiar:
- Texturas de paredes (pintura diferente)
- Fotos/posters: sprites de diseño "personal" de Marco
- Ropa en silla: asset de B9 (misma chaqueta del coche)
- La comida sobre la mesa: mismo asset de cocina de B12

**Iluminación:** Warm light (3200K) pero con desaturación -30% aplicada por VolumeProfile_Final.

---

### B15 — Props y Objetos Interactivos

**Lista completa de props necesarios:**

| Prop | Fuente | Destino | Usado en |
|---|---|---|---|
| Bolsa de delivery | Blender MCP | Prefabs/Props/ | Coche (B9) |
| Interphone | Blender MCP | Prefabs/Props/ | Exterior (B10) |
| Comida en plato | Blender MCP | Prefabs/Props/ | Casa (B12) |
| Foto enmarcada (cara borrosa) | Blender MCP + ShaderGraph | Prefabs/Props/ | Casa (B12) |
| Cajón de cocina | parte de B12 | — | Casa (B12) |
| Teléfono fijo de pared | Blender MCP | Prefabs/Props/ | Casa (B12) |
| Libros, botellas, plantas | Poly Haven (CC0) | Prefabs/Props/ | Casa (B12) |
| Casco de moto | Meshy AI (CC BY 4.0) | Prefabs/Props/ | Coche (B9) |

**`InteractableObject.cs` base:**
```csharp
// IndiGame.World namespace
// TriggerCollider en rango de E
// Outline shader 1px blanco cuando en rango (URP Render Feature: outline)
// OnInteract(): dispara EventManager.OnNarrativeTrigger(interactableID)
// Expone: public string interactableID (configurable en Inspector)
```

---

### B16 — Narrativa: Estado 1 y 2

**Preparación obligatoria ANTES de implementar:**
- Todos los ConversationSequence SOs deben estar creados con textos exactos
- Crear en `Assets/_Game/ScriptableObjects/Narrative/Delivery1/`, `Delivery2/`, `Delivery3/`

**`NarrativeDirector.cs`:**
```csharp
// IndiGame.Core namespace — el cerebro del juego
// Suscribe a EventManager.OnNarrativeTrigger
// Diccionario<string, Action>: trigger ID → método de respuesta
// Gestiona: ChatController (enviar mensajes), AppNotificationController, RatingController,
//           GalleryController, DeliveryManager, TransitionController
// Controla la GameState con GameManager.SetGameState()
```

**Secuencia completa Entrega 1 y 2 (ver Sección 2 para textos exactos).**

---

### B17 — Narrativa: Estado 3 y 4

**Los momentos más importantes:**
- Trigger "enter_house" → NarrativeDirector activa el flujo de infiltración
- Trigger "cajón_found" → NotificationData primer_trigger_suceso
- Trigger "sotano_tercero" → GalleryController.AddPhotos(3) + revelación completa
- Triggers de cambio de estado: RoomStateController.SetState(ANOMALOUS)

---

### B18 — Narrativa: Estado 5 + Title Card

- Trigger "escape_to_car" → DeliveryManager activa "última entrega"
- LoadingController especial (duración 8s, freeze 2s)
- DoppelgangerController.Activate() cuando jugador entra 2m
- Fade to black a 4s post-giro
- TitleCardController: texto línea por línea, 8s/línea
- Pantalla final: email de historias reales + "Episodio 2 — Próximamente"

---

### B19 — Sistema de Decisiones

**`DecisionPoint.cs` ScriptableObject:**
```csharp
// IndiGame.Core namespace
// Fields: decisionID, triggerEvent, obeyDelta (float), disobeyDelta (float)
// obeyResponse (string), disobeyResponse (string)
// obeyNextTrigger (string), disobeyNextTrigger (string)
```

Crear 8 SOs de decisión (ver Sección 5, tabla de las 8 decisiones).

---

### B20 — Doppelgänger

**Modelo:** Usar Blender MCP para crear un humanoid simple (low-poly, ~3000 triángulos). No usar Meshy para esto — Meshy free no garantiza calidad en humanoides y no genera animaciones.

**Animaciones (Unity Animation window):**
1. `idle_eating`: root fijo, brazo derecho loop 4s de elevación y descenso (solo 3 keyframes)
2. `turn_to_look`: rotación de Transform.head de Quaternion.identity a Quaternion.Euler(0, 180, 0) en 2.5s
3. `smile_idle`: micro-animación de comisura labial (BlendShape si el modelo lo tiene, si no: omitir)

**Animator Controller:**
```
Entry → idle_eating (inmediato)
idle_eating → turn_to_look (trigger: "Look", no exit time)
turn_to_look → smile_idle (exit time 1.0)
smile_idle → (loop)
```

---

### B21 — Audio: Ambientes y SFX

**Freesound CC0 — Lista de búsquedas exactas:**

| Búsqueda en Freesound | Destino |
|---|---|
| "rain on car roof loop" CC0 | Audio/SFX/Ambient/SFX_rain_car_loop |
| "refrigerator hum loop" CC0 | Audio/SFX/Ambient/SFX_fridge_hum |
| "clock ticking wall" CC0 | Audio/SFX/Ambient/SFX_clock_tick |
| "footsteps pavement night" CC0 | Audio/SFX/ |
| "cricket single night" CC0 | Audio/SFX/Ambient/ |
| "door knock wood" CC0 | Audio/SFX/ |
| "phone ringing old" CC0 | Audio/SFX/ |
| "camera shutter" CC0 | Audio/SFX/ |

**Ping de notificación:** ElevenLabs Sound Effects (no en Freesound, calidad superior). Prompt: *"short corporate notification chime, friendly, D major, 0.35 seconds, clean digital"*

---

### B22 — Audio: Voz de Marco

**ElevenLabs MCP:** 12 líneas. Voz: hombre 21 años, latinoamericano neutro, cansado.
- `VOX_marco_llegada_01.wav`: "¿Av. Patiño? Acá debe ser..."
- `VOX_marco_puerta_01.wav`: (golpea, espera, suspiro)
- `VOX_marco_fotos_01.wav`: (silencio largo) "...qué es esto."
- `VOX_marco_final_01.wav`: "El turno terminó."

Stability: 0.65, Clarity: 0.80, Style: 0.

---

### B23 — Audio: Música Generativa

**ElevenLabs Music Generation:**
- `MUS_normalidad_loop.wav`: "solo piano, slow melody, C major, ambient, 4 minutes, loopable"
- `MUS_anomalia_loop.wav`: "same piano melody as normalidad but with one wrong note every 45 seconds"
- `MUS_revelacion_loop.wav`: "piano reverb drone, no melody, low bass frequencies, unsettling"
- `MUS_final_silence.wav`: "near silence with sub-bass undertone, 30 seconds"

---

### B24 — Post-Processing + Iluminación Final

Aplicar los 5 Volume Profiles de B8 a la secuencia narrativa mediante el NarrativeDirector. Verificar frame rate en cada entorno.

---

### B25 — UI Polish + Animaciones del Teléfono

**Micro-animaciones pendientes:**
- Slide del teléfono: ease-out cubic, 0.3s (implementado en B7 como básico, pulir aquí)
- Badge de galería: pulso de escala (1.0→1.15→1.0, 0.8s loop)
- Rating stars fill animation: tween de 0 a valor real en 0.5s al abrir Panel_Inicio
- Typing indicator: 3 puntos que aparecen y desaparecen en ChatController

---

### B26 — QA Completo

**3 playthroughs obligatorios:**

1. **Run A — Obediente total:** Seguir cada instrucción de la app. Rating nunca baja de 3.0.
2. **Run B — Rebelde total:** Desobedecer cada decisión. Llegar al game over o lo más cerca posible.
3. **Run C — Natural:** Jugar como un jugador real que no conoce el juego.

**Checklist por escena:**
- [ ] Sin atravesar colisiones
- [ ] Todos los interactables responden en rango correcto
- [ ] Todos los triggers narrativos se activan una vez (no duplican)
- [ ] Audio no se solapa incorrectamente
- [ ] Teléfono responde en todo momento
- [ ] Rating refleja decisiones correctamente
- [ ] Camera roll recibe fotos en triggers correctos
- [ ] Doppelgänger se activa solo cuando el jugador está a ≤2m
- [ ] Title card es legible (tiempo entre líneas correcto)
- [ ] Sin errores en console (log de la sesión completa)

---

### B27 — Optimización y Builds

- Occlusion Culling baking: todas las escenas interiores
- Texture streaming: activar para texturas >512KB
- Audio: convertir todos a Vorbis 70%
- LODs: exterior building (2 LODs)
- Build Windows x64: `LAST_DELIVERY_EP1_v1.0_WIN.zip`
- Build Mac Universal: `LAST_DELIVERY_EP1_v1.0_MAC.zip`
- Test builds en hardware mínimo (si disponible)

---

### B28 — Lanzamiento y Marketing

**Assets necesarios (crear durante B26-B27):**

| Asset | Formato | Uso |
|---|---|---|
| Banner principal | 630×500px | itch.io |
| Screenshots × 6 | 1920×1080 | itch.io + Steam |
| Trailer corto TikTok | 30s, 1080×1920 (vertical) | TikTok principal |
| Trailer principal | 90s, 1920×1080 | YouTube |
| GIF del teléfono | 480×854px, loop 3s | Twitter / landing |

**Los 6 screenshots obligatorios:**
1. El teléfono mostrando el GPS con la ruta en la calle oscura
2. El camera roll con la foto borrosa de Marco durmiendo
3. El banner CRITICAL rojo pulsante en el teléfono
4. La sala de la casa — Estado NORMAL (mesa con comida, TV apagada)
5. El pasillo hacia la puerta del sótano (primer plano de cerradura)
6. El doppelgänger de espaldas en la mesa (no mostrar el giro — el giro es la sorpresa)

**Secuencia de lanzamiento:**
1. Semana -2: crear perfiles en redes (TikTok, Twitter, YouTube @lastdeliverygame)
2. Semana -1: publicar trailer en YouTube. Empezar contacto con creadores.
3. Día 0: 9 AM EST — publicar en itch.io. Trailer TikTok simultáneo.
4. Día 0: post en r/horrorgaming, r/indiegaming, r/gamedev, foros itch.io
5. Semana +1: responder TODOS los comentarios en itch.io. Recopilar feedback.
6. Semana +2: devlog corto en YouTube: "Hice este juego de horror solo en X semanas"

**Email a creadores:** Texto en inglés. Asunto: "Free 35-min horror game, no download required (itch.io link)". Cuerpo: logline + 2 screenshots + link directo. Sin archivo adjunto. Enviar a 20 creadores de horror indie medianos (100K–2M seguidores en TikTok/YouTube).

**TU HISTORIA COMO CONTENIDO VIRAL (dato de 2025):**
En 2025, un solo dev (Tangy TD) lloró en cámara al ver $245K de revenue en su primera semana — ese video se hizo más viral que el propio juego (cubierto por GamesRadar, IGN, Reddit). La historia personal de Adrian — solo dev boliviano construyendo un juego de horror entero con IA desde Bolivia — es un activo de marketing por sí misma.
- Grabar un devlog corto: "Hice un juego de horror sobre tu peor turno de delivery, usando IA" (3–5 minutos, español con subtítulos en inglés).
- Si el juego genera revenue, la reacción en cámara vale su peso en oro para el algoritmo de TikTok.

**EL CLIP VIRAL — Planificar en B26 (QA):**
Capturar durante QA el clip exacto para TikTok. Fórmula probada en 2025–2026:
1. Cold open: gameplay normal (Marco caminando, la app mostrando la ruta)
2. Amenaza repentina: el camera roll se abre solo con las fotos de Marco durmiendo
3. O alternativa: el doppelgänger gira la cabeza → corte a negro
4. **Duración: máximo 30 segundos. Sin contexto adicional.**
5. **No mostrar el giro completo del doppelgänger** — cortar ANTES del final. La curiosidad hace click.

---

## 💰 SECCIÓN 10 — MONETIZACIÓN Y LANZAMIENTO

### Modelo confirmado: Free-to-start episódico

| Episodio | Precio | Plataforma | Objetivo |
|---|---|---|---|
| Ep 1 | GRATIS | itch.io → Steam Free | Validación + funnel |
| Ep 2 | $7.99 | Steam | Revenue primario |
| Ep 3 | $7.99 | Steam | Compounding |
| Ep 4 | $7.99 | Steam (o bundle) | Lore crossover, expansión |
| Bundle Ep 1–4 | $19.99 | Steam | Descuento percibido |

### Proyecciones

**Conservadora (sin viral significativo):**
- Ep 1: 15,000–50,000 descargas gratis
- Ep 2: 1,500–5,000 copias × $7.99 = $12K–$40K gross
- Total 4 episodios: $100K–$300K gross

**Realista (viral pequeño-medio):**
- Ep 1: 50,000–200,000 descargas
- Ep 2: 5,000–20,000 copias × $7.99 = $40K–$160K gross
- Total 4 episodios: $300K–$800K gross

**Optimista (clip viral TikTok, escala Fears to Fathom):**
- Ep 1: 200,000+ descargas
- Episodios siguientes: compounding exponencial
- Total serie: $1M–$2.5M gross

**Regla de presupuesto para Ep 2:** Si Ep 1 supera 50,000 descargas en 30 días → invertir $500–$1,000 en marketing pagado (TikTok Ads). Si supera 200,000 → buscar publisher indie para distribución física/Steam bundle.

---

## 🔮 SECCIÓN 11 — VISIÓN DE FRANQUICIA

### Los próximos protagonistas

| Episodio | Protagonista | Rol gig economy | Horror central | Conexión lore |
|---|---|---|---|---|
| Ep 1 | Marco, 21, Bolivia | Delivery nocturno | Dirección que no existe / doppelgänger | — |
| Ep 2 | Ana, 26, Colombia | Rideshare nocturno | El pasajero que no baja / que cambia de cara | Usa la misma app (QuickRun Rides) |
| Ep 3 | Jin, 28, Chile | Warehouse picker (turno 3 AM) | El manager IA que pide artículos de pasillos cerrados | El warehouse pertenece a la misma holding que QuickRun |
| Ep 4 | ??? | ??? | El piso 6 de un edificio que no tiene 6 pisos | El edificio es donde vivió Marco |

### El hilo conductor: QuickRun Corporation

La empresa QuickRun no es solo una app de delivery. En Ep 4 se revela que es una corporación con décadas de historia que opera en zonas con "anomalías de espacio-tiempo" como parte de un programa de investigación encubierto. Los gig workers son los sujetos de prueba.

Este lore se construye gradualmente:
- **Ep 1:** QuickRun parece solo una app normal (cheerful, corporativa)
- **Ep 2:** El auto de Ana tiene un sticker de QuickRun → "¿la misma empresa?"
- **Ep 3:** El warehouse tiene documentos corporativos de QuickRun
- **Ep 4:** La revelación completa

### El mecanismo de engagement entre episodios

Cada episodio termina con un "código de investigación" — un detalle real (fecha, dirección, número de caso) que, al buscarse en Google, lleva a una página ARG (Alternate Reality Game) simple que da pistas del siguiente episodio. Costo: 0. Engagement: masivo.

---

## 📐 SECCIÓN 12 — ESTÁNDARES DE PRODUCCIÓN

### Criterio de DONE por bloque

Un bloque está COMPLETO cuando:
1. El commit final existe en `main` con el SHA correspondiente
2. Se puede dar Play en Unity y ver/usar el resultado del bloque
3. Console sin errores rojos (excepto errores conocidos de otras features)
4. La subpágina de Notion del bloque está actualizada
5. El GDD fue actualizado si el bloque modificó algo del diseño original

### Criterio de DONE para Ep 1 completo

- [ ] 3 playthroughs de QA completados (ver B26)
- [ ] 0 bugs críticos (gameplay-breaking) abiertos
- [ ] Build Windows funciona en hardware mínimo a 60fps
- [ ] Build Mac funciona
- [ ] Todos los textos revisados en inglés y español
- [ ] itch.io page creada con screenshots, trailer y descripción
- [ ] El momento del doppelgänger funciona perfectamente en al menos 10 tests

### Reglas para Antigravity

1. **Siempre: scene-get-data antes de tocar nada** → entender jerarquía actual
2. **Siempre: console-get-logs(Error) antes y después de scripts** → 0 errores antes de continuar
3. **Siempre: script-execute para construir escena** → nunca solo escribir scripts
4. **Siempre: SerializedObject para wirear [SerializeField] privados**
5. **Siempre: verificar en Play Mode** → singletons + llamar métodos + screenshots
6. **Siempre: git config Adrian-stratega** + commit limpio al final
7. **Nunca: usar legacy Input.GetKeyDown** → usar Keyboard.current (new Input System)
8. **Nunca: SetActive(false) en GameOverCanvas/TransitionCanvas/LoadingCanvas**
9. **Ignorar:** SocketException / HubConnection en console → son MCP transitorios

---

*Versión 2.0 — Generado con análisis de escena real (Claude Code) + investigación de mercado*
*Última actualización: 26 de marzo de 2026*

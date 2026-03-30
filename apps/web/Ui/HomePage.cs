namespace KnowBase.Web.Ui;

public static class HomePage
{
    public static string Render() =>
        """
        <!DOCTYPE html>
        <html lang="en">
        <head>
          <meta charset="utf-8" />
          <meta name="viewport" content="width=device-width, initial-scale=1" />
          <title>KnowBase</title>
          <style>
            :root {
              color-scheme: light;
              --bg: #f3efe5;
              --panel: rgba(255, 252, 247, 0.88);
              --ink: #1f2933;
              --muted: #5b6770;
              --accent: #0d5c63;
              --accent-soft: #d8ecec;
              --line: rgba(13, 92, 99, 0.16);
              --shadow: 0 24px 60px rgba(31, 41, 51, 0.14);
            }

            * { box-sizing: border-box; }

            body {
              margin: 0;
              min-height: 100vh;
              font-family: "Segoe UI", "Aptos", sans-serif;
              color: var(--ink);
              background:
                radial-gradient(circle at top left, rgba(13, 92, 99, 0.18), transparent 28rem),
                linear-gradient(160deg, #f8f4ea 0%, #efe7d5 54%, #e7eef0 100%);
            }

            main {
              width: min(960px, calc(100% - 2rem));
              margin: 0 auto;
              padding: 3rem 0 4rem;
            }

            .hero {
              margin-bottom: 1.5rem;
            }

            .eyebrow {
              display: inline-block;
              padding: 0.35rem 0.7rem;
              border-radius: 999px;
              background: var(--accent-soft);
              color: var(--accent);
              font-size: 0.8rem;
              font-weight: 700;
              letter-spacing: 0.08em;
              text-transform: uppercase;
            }

            h1 {
              margin: 0.9rem 0 0.6rem;
              font-size: clamp(2.4rem, 5vw, 4.5rem);
              line-height: 0.95;
              letter-spacing: -0.04em;
              max-width: 11ch;
            }

            .hero p {
              max-width: 42rem;
              margin: 0;
              color: var(--muted);
              font-size: 1.05rem;
            }

            .panel {
              background: var(--panel);
              border: 1px solid var(--line);
              border-radius: 1.5rem;
              box-shadow: var(--shadow);
              backdrop-filter: blur(18px);
            }

            .composer {
              padding: 1.25rem;
            }

            textarea {
              width: 100%;
              min-height: 8rem;
              resize: vertical;
              border: 1px solid var(--line);
              border-radius: 1rem;
              padding: 1rem;
              font: inherit;
              color: var(--ink);
              background: rgba(255, 255, 255, 0.78);
            }

            .composer-footer {
              display: flex;
              gap: 1rem;
              justify-content: space-between;
              align-items: center;
              margin-top: 1rem;
            }

            .hint {
              color: var(--muted);
              font-size: 0.95rem;
            }

            button {
              border: 0;
              border-radius: 999px;
              padding: 0.9rem 1.3rem;
              font: inherit;
              font-weight: 700;
              color: white;
              background: linear-gradient(135deg, #0d5c63, #144552);
              cursor: pointer;
            }

            button:disabled {
              opacity: 0.6;
              cursor: wait;
            }

            .results {
              margin-top: 1.5rem;
              padding: 1.25rem;
            }

            .results.hidden {
              display: none;
            }

            .status {
              font-size: 0.9rem;
              font-weight: 700;
              text-transform: uppercase;
              letter-spacing: 0.08em;
              color: var(--accent);
            }

            .answer {
              margin-top: 0.9rem;
              font-size: 1.05rem;
              line-height: 1.6;
            }

            .citations {
              margin-top: 1.25rem;
              display: grid;
              gap: 0.85rem;
            }

            .citation {
              padding: 1rem;
              border-radius: 1rem;
              border: 1px solid var(--line);
              background: rgba(255, 255, 255, 0.82);
            }

            .citation h2 {
              margin: 0;
              font-size: 1rem;
            }

            .citation-meta {
              margin-top: 0.35rem;
              font-size: 0.88rem;
              color: var(--muted);
            }

            .citation p {
              margin: 0.75rem 0 0;
              color: var(--ink);
            }

            code {
              font-family: "Cascadia Code", "Consolas", monospace;
              font-size: 0.92em;
            }

            @media (max-width: 720px) {
              .composer-footer {
                align-items: stretch;
                flex-direction: column;
              }

              button {
                width: 100%;
              }
            }
          </style>
        </head>
        <body>
          <main>
            <section class="hero">
              <span class="eyebrow">Grounded v1 Slice</span>
              <h1>Ask KnowBase about past project work.</h1>
              <p>
                This first slice uses a mocked internal knowledge base so we can validate the app flow,
                grounding behavior, and citation UX before wiring in the real ingestion and local model stack.
              </p>
            </section>

            <section class="panel composer">
              <label for="question"><strong>Project question</strong></label>
              <textarea id="question" placeholder="Example: Have we done a wastewater pump station rehab with corrosion issues?"></textarea>
              <div class="composer-footer">
                <div class="hint">Tip: include client, project type, discipline, or a problem description.</div>
                <button id="send" type="button">Search internal knowledge</button>
              </div>
            </section>

            <section id="results" class="panel results hidden" aria-live="polite">
              <div id="status" class="status"></div>
              <div id="answer" class="answer"></div>
              <div id="citations" class="citations"></div>
            </section>
          </main>

          <script>
            const questionElement = document.getElementById("question");
            const sendButton = document.getElementById("send");
            const resultsElement = document.getElementById("results");
            const statusElement = document.getElementById("status");
            const answerElement = document.getElementById("answer");
            const citationsElement = document.getElementById("citations");

            async function submitQuestion() {
              const question = questionElement.value.trim();

              if (!question) {
                statusElement.textContent = "Question required";
                answerElement.textContent = "Add a project question to run the first grounded slice.";
                citationsElement.innerHTML = "";
                resultsElement.classList.remove("hidden");
                return;
              }

              sendButton.disabled = true;
              statusElement.textContent = "Searching sample corpus";
              answerElement.textContent = "Running retrieval and composing a grounded response...";
              citationsElement.innerHTML = "";
              resultsElement.classList.remove("hidden");

              try {
                const response = await fetch("/api/chat", {
                  method: "POST",
                  headers: { "Content-Type": "application/json" },
                  body: JSON.stringify({ question })
                });

                const payload = await response.json();

                if (!response.ok) {
                  throw new Error(payload.title || "The request could not be completed.");
                }

                statusElement.textContent = payload.grounded ? "Grounded response" : "No grounded match";
                answerElement.textContent = payload.answer;
                citationsElement.innerHTML = (payload.citations || []).map(citation => `
                  <article class="citation">
                    <h2>${citation.title}</h2>
                    <div class="citation-meta">
                      <span>${citation.documentType}</span>
                      <span> - score ${citation.score.toFixed(2)}</span>
                    </div>
                    <p>${citation.excerpt}</p>
                    <p><code>${citation.canonicalUri}</code></p>
                  </article>
                `).join("");
              } catch (error) {
                statusElement.textContent = "Request failed";
                answerElement.textContent = error.message || "Unexpected error.";
                citationsElement.innerHTML = "";
              } finally {
                sendButton.disabled = false;
              }
            }

            sendButton.addEventListener("click", submitQuestion);
            questionElement.addEventListener("keydown", event => {
              if ((event.ctrlKey || event.metaKey) && event.key === "Enter") {
                submitQuestion();
              }
            });
          </script>
        </body>
        </html>
        """;
}
